document.addEventListener('DOMContentLoaded', function () {

    const logoutForms = document.querySelectorAll('form[action*="Logout"]');
    logoutForms.forEach(form => {
        form.addEventListener('submit', function (e) {
            e.preventDefault();
            Swal.fire({
                title: 'Leaving so soon?',
                text: "You will need to log in again to access the portal.",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonText: '<i class="fas fa-sign-out-alt"></i> Yes, log me out',
                cancelButtonText: 'Cancel',
                buttonsStyling: false,
                background: '#ffffff',
                color: '#1a365d',
                backdrop: `rgba(10, 25, 47, 0.8)`,
                customClass: {
                    popup: 'modern-popup',
                    htmlContainer: 'modern-html-container', 
                    confirmButton: 'modern-btn-common modern-confirm-btn-danger',
                    cancelButton: 'modern-btn-common modern-cancel-btn'
                }
            }).then((result) => {
                if (result.isConfirmed) {
                    form.submit();
                }
            });
        });
    });

    const successToast = document.getElementById('toastNotification');
    const errorToast = document.getElementById('toastNotificationError');

    if (successToast) {
        setTimeout(() => {
            successToast.classList.add('slide-out');
            setTimeout(() => successToast.remove(), 400);
        }, 3000);
    }

    if (errorToast) {
        setTimeout(() => {
            errorToast.classList.add('slide-out');
            setTimeout(() => errorToast.remove(), 400);
        }, 3000);
    }

    const tables = {
        'students': document.getElementById('table-students'),
        'supervisors': document.getElementById('table-supervisors'),
        'admins': document.getElementById('table-admins')
    };

    const dropdownItems = document.querySelectorAll('.custom-rounded-menu .dropdown-item');
    const dropdownBtn = document.getElementById('userRoleDropdown');

    dropdownItems.forEach(item => {
        item.addEventListener('click', function (e) {
            e.preventDefault();

            dropdownBtn.innerText = this.innerText;

            dropdownItems.forEach(i => i.classList.remove('active'));
            this.classList.add('active');

            const selectedRole = this.dataset.value;

            Object.values(tables).forEach(t => {
                if (t) t.classList.add('d-none');
            });

            if (tables[selectedRole]) {
                tables[selectedRole].classList.remove('d-none');
            }
        });
    });

    const modalRoleSelect = document.getElementById('modalRoleSelect');
    const fieldStudent = document.getElementById('field-student');
    const fieldSupervisorId = document.getElementById('field-supervisor-id');
    const fieldSupervisorQuota = document.getElementById('field-supervisor-quota');

    if (modalRoleSelect) {
        modalRoleSelect.addEventListener('change', function () {

            fieldStudent.classList.add('d-none');
            fieldSupervisorId.classList.add('d-none');
            fieldSupervisorQuota.classList.add('d-none');

            if (this.value === 'Student') {
                fieldStudent.classList.remove('d-none');
            } else if (this.value === 'Supervisor') {
                fieldSupervisorId.classList.remove('d-none');
                fieldSupervisorQuota.classList.remove('d-none');
            }
        });
    }

    const togglePasswordIcon = document.getElementById('togglePasswordIcon');
    const modalPasswordInput = document.getElementById('modalPasswordInput');

    if (togglePasswordIcon && modalPasswordInput) {
        togglePasswordIcon.addEventListener('click', function () {
            const isPassword = modalPasswordInput.getAttribute('type') === 'password';
            modalPasswordInput.setAttribute('type', isPassword ? 'text' : 'password');
            this.classList.toggle('fa-eye');
            this.classList.toggle('fa-eye-slash');
        });
    }

    const addUserForm = document.getElementById('addUserForm');
    const modalInstance = new bootstrap.Modal(document.getElementById('addUserModal'))

    document.querySelector('.btn-add-user').addEventListener('click', () => {
        addUserForm.reset();
        document.getElementById('editUserId').value = '';
        document.querySelector('.custom-modal-header h2').innerText = 'Add User';
        document.querySelector('#addUserForm button[type="submit"]').innerText = 'Add';

        document.getElementById('modalRoleSelect').disabled = false;
        document.getElementById('addStudentId').readOnly = false;
        document.getElementById('addSupervisorId').readOnly = false;
        document.getElementById('addQuota').readOnly = false;

        document.getElementById('modalPasswordInput').required = true;
        document.getElementById('modalPasswordInput').placeholder = '';

        document.getElementById('modalRoleSelect').dispatchEvent(new Event('change'));
    });

    document.querySelectorAll('.icon-edit').forEach(btn => {
        btn.addEventListener('click', function () {
            const data = this.dataset;

            document.getElementById('editUserId').value = data.userid;
            document.getElementById('addFirstName').value = data.firstname;
            document.getElementById('addLastName').value = data.lastname;
            document.getElementById('addEmail').value = data.email;

            const roleSelect = document.getElementById('modalRoleSelect');
            roleSelect.value = data.role;
            roleSelect.dispatchEvent(new Event('change'));

            roleSelect.disabled = true;

            if (data.role === 'Student') {
                document.getElementById('addStudentId').value = data.studentid;
                document.getElementById('addStudentId').readOnly = true;
            } else if (data.role === 'Supervisor') {
                document.getElementById('addSupervisorId').value = data.supervisorid;
                document.getElementById('addSupervisorId').readOnly = true;
                document.getElementById('addQuota').value = data.quota;
                document.getElementById('addQuota').readOnly = true;
            }

            document.getElementById('modalPasswordInput').required = false;
            document.getElementById('modalPasswordInput').placeholder = '(Leave blank to keep current)';
            document.getElementById('modalPasswordInput').value = '';

            document.querySelector('.custom-modal-header h2').innerText = 'Edit User';
            document.querySelector('#addUserForm button[type="submit"]').innerText = 'Update';

            modalInstance.show();
        });
    });

    if (addUserForm) {
        addUserForm.addEventListener('submit', async function (e) {
            e.preventDefault();

            const editUserId = document.getElementById('editUserId').value;
            const isEditMode = editUserId !== '';
            const role = document.getElementById('modalRoleSelect').value;

            if (!addUserForm.checkValidity()) {
                addUserForm.reportValidity();
                return;
            }

            if (role == 'Student') {
                const studentId = document.getElementById('addStudentId').value.trim();
                if (!studentId) {
                    Swal.fire({
                        icon: 'warning',
                        title: 'Missing Info',
                        text: 'Please enter a Student ID.',
                        buttonsStyling: false,
                        background: '#ffffff',
                        color: '#1a365d',
                        backdrop: `rgba(10, 25, 47, 0.8)`,
                        customClass: {
                            popup: 'modern-popup',
                            htmlContainer: 'modern-html-container', 
                            confirmButton: 'modern-btn-common modern-cancel-btn'
                        }
                    });
                    return;
                }
            } else if (role === 'Supervisor') {
                const supId = document.getElementById('addSupervisorId').value.trim();
                const quota = document.getElementById('addQuota').value;

                if (!supId) {
                    Swal.fire({
                        icon: 'warning',
                        title: 'Missing Info',
                        text: 'Please enter a Supervisor ID.',
                        buttonsStyling: false,
                        background: '#ffffff',
                        color: '#1a365d',
                        backdrop: `rgba(10, 25, 47, 0.8)`,
                        customClass: {
                            popup: 'modern-popup',
                            htmlContainer: 'modern-html-container', 
                            confirmButton: 'modern-btn-common modern-cancel-btn'
                        }
                    });
                    return;
                }
                if (!quota || parseInt(quota) <= 0) {
                    Swal.fire({
                        icon: 'warning',
                        title: 'Invalid Quota',
                        text: 'Project Quota must be greater than zero.',
                        buttonsStyling: false,
                        background: '#ffffff',
                        color: '#1a365d',
                        backdrop: `rgba(10, 25, 47, 0.8)`,
                        customClass: {
                            popup: 'modern-popup',
                            htmlContainer: 'modern-html-container', 
                            confirmButton: 'modern-btn-common modern-cancel-btn'
                        }
                    });
                    return;
                }
            }

            const targetUrl = isEditMode ? '/Admin/UpdateUser' : '/Admin/AddNewUser';

            const payload = {
                FirstName: document.getElementById('addFirstName').value,
                LastName: document.getElementById('addLastName').value,
                Email: document.getElementById('addEmail').value,
                Password: document.getElementById('modalPasswordInput').value
            };

            if (!isEditMode) {
                payload.Role = role;
                payload.StudentId = role === 'Student' ? document.getElementById('addStudentId').value : null;
                payload.SupervisorId = role === 'Supervisor' ? document.getElementById('addSupervisorId').value : null;
                payload.ProjectQuota = role === 'Supervisor' ? parseInt(document.getElementById('addQuota').value) || null : null;
            } else {
                payload.UserId = parseInt(editUserId);
            }

            try {
                await fetch(targetUrl, {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(payload)
                });

                window.location.reload();
            }
            catch (error) {
                console.error('Error:', error);
                Swal.fire({
                    icon: 'error',
                    title: 'System Error',
                    text: 'An error occurred while communicating with the server.',
                    buttonsStyling: false,
                    background: '#ffffff',
                    color: '#1a365d',
                    backdrop: `rgba(10, 25, 47, 0.8)`,
                    customClass: {
                        popup: 'modern-popup',
                        htmlContainer: 'modern-html-container', 
                        confirmButton: 'modern-btn-common modern-cancel-btn'
                    }
                });
            }
        })
    }
});

document.querySelectorAll('.icon-toggle').forEach(btn => {
    btn.addEventListener('click', function () {
        const userId = this.dataset.userid;
        const userName = this.dataset.name;
        const isActive = this.dataset.isactive === 'true';

        const actionText = isActive ? 'Deactivate' : 'Reactivate';
        const iconType = isActive ? 'warning' : 'info';

        const confirmBtnClass = isActive ? 'modern-confirm-btn-danger' : 'modern-confirm-btn';

        Swal.fire({
            title: `${actionText} User?`,
            text: `Are you sure you want to ${actionText.toLowerCase()} ${userName}?`,
            icon: iconType,
            showCancelButton: true,
            confirmButtonText: `Yes, ${actionText}!`,
            cancelButtonText: 'Cancel',
            buttonsStyling: false,
            background: '#ffffff',
            color: '#1a365d',
            backdrop: `rgba(10, 25, 47, 0.8)`,
            customClass: {
                popup: 'modern-popup',
                htmlContainer: 'modern-html-container', 
                confirmButton: `modern-btn-common ${confirmBtnClass}`,
                cancelButton: 'modern-btn-common modern-cancel-btn'
            }
        }).then(async (result) => {
            if (result.isConfirmed) {
                try {
                    await fetch('/Admin/ToggleUserStatus', {
                        method: 'POST',
                        headers: { 'Content-Type': 'application/json' },
                        body: JSON.stringify({ UserId: parseInt(userId) })
                    });

                    window.location.reload();

                } catch (error) {
                    console.error('Error', error);
                    Swal.fire({
                        icon: 'error',
                        title: 'Server Error',
                        text: 'Could not connect to the server.',
                        buttonsStyling: false,
                        background: '#ffffff',
                        color: '#1a365d',
                        backdrop: `rgba(10, 25, 47, 0.8)`,
                        customClass: {
                            popup: 'modern-popup',
                            htmlContainer: 'modern-html-container', 
                            confirmButton: 'modern-btn-common modern-cancel-btn'
                        }
                    });
                }
            }
        })
    })
})
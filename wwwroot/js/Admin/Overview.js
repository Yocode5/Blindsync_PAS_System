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
});

let currentProjectId = null;

function openReassignModal(projectId, studentName, projectName, currentSupervisor) {
    currentProjectId = projectId;
    document.getElementById('modalStudentName').innerText = studentName;
    document.getElementById('modalProjectName').innerText = projectName;
    document.getElementById('modalCurrentSupervisor').innerText = currentSupervisor;
    document.getElementById('reassignModal').style.display = 'flex';
}

function closeReassignModal() {
    document.getElementById('reassignModal').style.display = 'none';
    document.getElementById('newSupervisorSelect').value = "";
    currentProjectId = null;
}

async function submitReassign() {
    const newSupervisorId = document.getElementById('newSupervisorSelect').value;

    if (newSupervisorId === "") {
        Swal.fire({
            icon: 'warning',
            title: 'Missing Info',
            text: 'Please select a new supervisor from the list.',
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

    try {
        await fetch('/Admin/ReassignSupervisor', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                ProjectId: parseInt(currentProjectId),
                SupervisorId: parseInt(newSupervisorId)
            })
        });

        window.location.reload();

    } catch (error) {
        console.error("Error:", error);
        Swal.fire({
            icon: 'error',
            title: 'System Error',
            text: 'Something went wrong. Please try again.',
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
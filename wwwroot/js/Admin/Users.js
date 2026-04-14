document.addEventListener('DOMContentLoaded', function () {

    const roleFilter = document.getElementById('userRoleFilter');

    const tables = {
        'students': document.getElementById('table-students'),
        'supervisors': document.getElementById('table-supervisors'),
        'admins': document.getElementById('table-admins')
    };

    roleFilter.addEventListener('change', function () {
        const selectedRole = this.value;

        Object.values(tables).forEach(t => {
            if (t) t.classList.add('d-none');
        });

        if (tables[selectedRole]) {
            tables[selectedRole].classList.remove('d-none');
        }
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
});
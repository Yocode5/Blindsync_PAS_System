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
});
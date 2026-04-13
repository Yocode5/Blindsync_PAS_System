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
                confirmButtonColor: '#E67474', 
                cancelButtonColor: '#4A6282', 
                confirmButtonText: 'Yes, log me out!'
            }).then((result) => {
                if (result.isConfirmed) {
                    form.submit(); 
                }
            });
        });
    });
});
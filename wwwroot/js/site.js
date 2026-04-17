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
                    confirmButton: 'modern-confirm-btn',
                    cancelButton: 'modern-cancel-btn'
                }
            }).then((result) => {
                if (result.isConfirmed) {
                    form.submit();
                }
            });
        });
    });
});
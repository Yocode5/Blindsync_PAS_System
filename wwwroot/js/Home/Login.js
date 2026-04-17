document.getElementById('togglePassword').addEventListener('click', function () {
    const passwordInput = document.getElementById('passwordInput');
    const type = passwordInput.getAttribute('type') === 'password' ? 'text' : 'password';
    passwordInput.setAttribute('type', type);

    this.classList.toggle('bi-eye');
    this.classList.toggle('bi-eye-slash');
});

document.addEventListener("DOMContentLoaded", function () {
    const errorTrigger = document.getElementById('loginErrorTrigger');

    if (errorTrigger) {
        const message = errorTrigger.getAttribute('data-error');
        showCustomErrorToast(message);
    }
});

document.querySelector('form').addEventListener('submit', function (e) {
    if (!$(this).valid()) {
        return;
    }

    Swal.fire({
        title: 'Authenticating',
        html: '<span style="color: #64748b; font-size: 0.95rem;">Checking your credentials securely...</span>',
        allowOutsideClick: false,
        showConfirmButton: false,
        width: '380px',
        padding: '2.5em',
        background: '#ffffff',
        color: '#1a365d',
        backdrop: `rgba(10, 25, 47, 0.8)`,
        didOpen: () => {
            Swal.showLoading();
            const loader = Swal.getPopup().querySelector('.swal2-loader');
            if (loader) {
                loader.style.borderColor = '#244A75 transparent';
                loader.style.borderWidth = '4px';
            }
        }
    });
});

function showCustomErrorToast(message) {
    const existingToast = document.getElementById('toastNotificationError');
    if (existingToast) existingToast.remove();

    const toast = document.createElement('div');
    toast.id = 'toastNotificationError';
    toast.className = 'custom-toast error-toast';

    toast.innerHTML = `
        <i class="fas fa-exclamation-circle"></i>
        <span>${message}</span>
    `;

    document.body.appendChild(toast);

    setTimeout(() => {
        toast.style.animation = 'slideOutRight 0.4s ease-out forwards';
        setTimeout(() => toast.remove(), 400);
    }, 4000);
}
document.getElementById('togglePassword').addEventListener('click', function () {
    const passwordInput = document.getElementById('passwordInput');
    const type = passwordInput.getAttribute('type') === 'password' ? 'text' : 'password';
    passwordInput.setAttribute('type', type);

    this.classList.toggle('bi-eye');
    this.classList.toggle('bi-eye-slash');
});

document.addEventListener("DOMContentLoaded", function () {
    const errorDiv = document.getElementById('server-errors');
    if (errorDiv && errorDiv.innerText.trim() !== '') {
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'Invalid email or password!',
            confirmButtonColor: '#244A75'
        });
    }
});

document.querySelector('form').addEventListener('submit', function (e) {
    const email = document.getElementById('Email').value.trim();
    const password = document.getElementById('passwordInput').value.trim();

    // Check if the fields are empty
    if (!email || !password) {
        e.preventDefault(); // Stops the form from submitting
        Swal.fire({
            icon: 'warning',
            title: 'Hold up!',
            text: 'Please enter both your email and password.',
            confirmButtonColor: '#244A75'
        });
        return;
    }

    Swal.fire({
        title: 'Authenticating...',
        text: 'Checking your credentials',
        allowOutsideClick: false,
        showConfirmButton: false,
        didOpen: () => {
            Swal.showLoading();
        }
    });
})
function switchTab(tabId, element) {
    document.querySelectorAll('.tab-btn').forEach(btn => btn.classList.remove('active'));
    document.querySelectorAll('.tab-content').forEach(content => content.classList.remove('active'));

    element.classList.add('active');
    document.getElementById('tab-' + tabId).classList.add('active');
}

function toggleVisibility(inputId, iconElement) {
    const passwordInput = document.getElementById(inputId);

    if (passwordInput.type === "password") {
        passwordInput.type = "text";
        iconElement.classList.remove("fa-eye");
        iconElement.classList.add("fa-eye-slash");
    } else {
        passwordInput.type = "password";
        iconElement.classList.remove("fa-eye-slash");
        iconElement.classList.add("fa-eye");
    }
}

async function submitProfileForm(event) {
    event.preventDefault(); 
    event.stopImmediatePropagation(); 

    const form = event.target;
    const formData = new FormData(form);

    try {
        await fetch('/Home/UpdateProfile', {
            method: 'POST',
            body: formData
        });

        window.location.reload();

    } catch (error) {
        console.error("Profile Update Error: ", error);
        window.location.reload();
    }
}
document.addEventListener("DOMContentLoaded", function () {
    const toasts = document.querySelectorAll('.custom-toast');
    toasts.forEach(toast => {
        setTimeout(() => {
            toast.style.animation = "slideOutRight 0.4s ease-in forwards";
            setTimeout(() => { toast.remove(); }, 400);
        }, 3500);
    });

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
                    confirmButton: 'modern-confirm-btn-danger',
                    cancelButton: 'modern-cancel-btn'
                }
            }).then((result) => {
                if (result.isConfirmed) { form.submit(); }
            });
        });
    });
});

function openProposalModal(title, area, tech, abstract) {
    document.getElementById('modalTitle').innerText = title;
    document.getElementById('modalArea').innerText = area;
    document.getElementById('modalAbstractText').innerText = abstract;
    const techContainer = document.getElementById('modalTechStack');
    techContainer.innerHTML = '';
    if (tech) {
        tech.split(',').forEach(t => {
            if (t.trim() !== "") {
                const span = document.createElement('span');
                span.className = 'modal-tech-pill';
                span.innerText = t.trim();
                techContainer.appendChild(span);
            }
        });
    }
    document.getElementById('proposalModalOverlay').style.display = 'flex';
}

function closeProposalModal() { document.getElementById('proposalModalOverlay').style.display = 'none'; }

function closeProposalModalOutside(event) {
    if (event.target === document.getElementById('proposalModalOverlay')) { closeProposalModal(); }
}

function acceptProjectFromMyProjects(projectId) {
    Swal.fire({
        title: 'Accept Project?',
        text: "Are you sure you want to officially accept this project match?",
        icon: 'question',
        showCancelButton: true,
        confirmButtonText: 'Yes, accept it',
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
            fetch(`/Supervisors/AcceptProject?id=${projectId}`, { method: 'POST' })
                .then(response => { if (response.ok) { window.location.reload(); } else { showErrorToast('Failed to accept project.'); } })
                .catch(error => { showErrorToast('A network error occurred.'); });
        }
    });
}

function releaseProject(projectId) {
    Swal.fire({
        title: 'Release Project?',
        text: "This will return the project to the pending pool for other supervisors.",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, release it',
        cancelButtonText: 'Cancel',
        buttonsStyling: false,
        background: '#ffffff',
        color: '#1a365d',
        backdrop: `rgba(10, 25, 47, 0.8)`,
        customClass: {
            popup: 'modern-popup',
            confirmButton: 'modern-confirm-btn-danger',
            cancelButton: 'modern-cancel-btn'
        }
    }).then((result) => {
        if (result.isConfirmed) {
            fetch(`/Supervisors/ReleaseProject?id=${projectId}`, { method: 'POST' })
                .then(response => { if (response.ok) { window.location.reload(); } else { showErrorToast('Failed to release project.'); } })
                .catch(error => { showErrorToast('A network error occurred.'); });
        }
    });
}

function showErrorToast(msg) {
    Swal.fire({
        icon: 'error',
        title: 'Error',
        text: msg,
        buttonsStyling: false,
        customClass: { popup: 'modern-popup', confirmButton: 'modern-confirm-btn-danger' }
    });
}
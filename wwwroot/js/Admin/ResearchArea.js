document.addEventListener('DOMContentLoaded', function () {
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

async function addNewArea() {
    const input = document.getElementById('newAreaInput');
    const areaName = input.value.trim();

    try {
        const response = await fetch('/Admin/AddResearchArea', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ Name: areaName })
        });

        location.reload();
    } catch (error) {
        console.error("Error adding research area:", error);
    }
}

async function deleteArea(id) {
    Swal.fire({
        title: 'Are you sure?',
        text: "Remove this research area permanently?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, delete it',
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
    }).then(async (result) => {
        if (result.isConfirmed) {
            try {
                await fetch(`/Admin/DeleteResearchArea?id=${id}`, {
                    method: 'POST'
                });
                location.reload();
            } catch (error) {
                console.error("Error deleting research area:", error);
            }
        }
    });
}
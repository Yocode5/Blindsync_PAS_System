function openModal() {
    document.getElementById('proposalModal').style.display = 'flex';
}

function closeModal() {
    document.getElementById('proposalModal').style.display = 'none';
}

window.onclick = function (event) {
    let modal = document.getElementById('proposalModal');
    if (event.target == modal) {
        closeModal();
    }

    if (!event.target.closest('.custom-dropdown-container')) {
        document.getElementById('researchDropdown').classList.remove('open');
        document.getElementById('dropdownOptions').classList.remove('show');
    }
}

let techTags = [];
const techInput = document.getElementById('techInput');
const hiddenTechStack = document.getElementById('hiddenTechStack');
const tagContainer = document.getElementById('techTagContainer');

techInput.addEventListener('keydown', function (e) {
    if (e.key === 'Enter' || e.key === ',') {
        e.preventDefault();

        let tagText = techInput.value.trim().replace(',', '');

        if (tagText !== '' && !techTags.includes(tagText)) {
            techTags.push(tagText);
            renderTags();
        }

        techInput.value = '';
    }
});

function renderTags() {
    tagContainer.innerHTML = '';

    techTags.forEach((tag, index) => {
        let tagElement = document.createElement('span');
        tagElement.className = 'tech-tag';

        tagElement.innerHTML = `${tag} <i class="fas fa-times" style="cursor: pointer;" onclick="removeTag(${index})"></i>`;

        tagContainer.appendChild(tagElement);
    });

    hiddenTechStack.value = techTags.join(',');
}

function removeTag(index) {
    techTags.splice(index, 1);
    renderTags();
}

function toggleDropdown(event) {
    event.stopPropagation();

    const container = document.getElementById('researchDropdown');
    const options = document.getElementById('dropdownOptions');

    container.classList.toggle('open');
    options.classList.toggle('show');
}

function selectResearchArea(id, name) {
    document.getElementById('selectedResearchText').innerText = name;

    document.getElementById('hiddenResearchAreaId').value = id;

    document.getElementById('researchDropdown').classList.remove('open');
    document.getElementById('dropdownOptions').classList.remove('show');
}

document.addEventListener("DOMContentLoaded", function () {
    let toast = document.getElementById("toastNotification");
    if (toast) {
        setTimeout(function () {
            toast.style.animation = "slideOutRight 0.4s ease-in forwards";

            setTimeout(() => toast.remove(), 400);
        }, 3500);
    }
});

function confirmWithdrawal() {
    Swal.fire({
        title: 'Withdraw Proposal?',
        text: "Are you sure you want to withdraw this proposal? You will have to submit a new one.",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#b03d3d', 
        cancelButtonColor: '#2b4f6b',  
        confirmButtonText: 'Yes, withdraw it!',
        cancelButtonText: 'Cancel'
    }).then((result) => {
        if (result.isConfirmed) {
            document.getElementById('withdrawForm').submit();
        }
    });
}
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

function openEditModal(title, areaId, techStack, abstract) {
    document.getElementById('editFieldProjectTitle').value = title;
    document.getElementById('editFieldAbstract').value = abstract;

    const hiddenInput = document.getElementById('hiddenEditResearchAreaId');
    hiddenInput.value = areaId;

    const areaNameMap = {
        "1": "Artificial Intelligence",
        "2": "Software Engineering",
        "3": "Data Science",
        "4": "Cyber Security"
    };

    const selectedText = areaNameMap[areaId] || "Select Research Area";
    document.getElementById('editSelectedResearchText').innerText = selectedText;

    const tagContainer = document.getElementById('editTagContainerElement');
    tagContainer.innerHTML = '';

    if (techStack && techStack.trim() !== "") {
        const techs = techStack.split(',');
        techs.forEach(tech => {
            const trimmedTech = tech.trim();
            if (trimmedTech) {
                const span = document.createElement('span');
                span.className = 'tech-tag';
                span.innerHTML = `${trimmedTech} <i onclick="this.parentElement.remove()">&times;</i>`;
                tagContainer.appendChild(span);
            }
        });
    }

    document.getElementById('editProposalModal').style.display = 'flex';
}

function closeEditModal() {
    document.getElementById('editProposalModal').style.display = 'none';
}

function submitEdit() {
    const updatedTitle = document.getElementById('editFieldProjectTitle').value;
    const updatedArea = document.getElementById('editSelectedResearchText').innerText;

 
    const dashboardTitle = document.querySelector('.proposal-title-row h4');
    if (dashboardTitle) {
        dashboardTitle.innerText = updatedTitle;
    }
  
    const detailRows = document.querySelectorAll('.detail-row');
    detailRows.forEach(row => {
        if (row.innerText.includes("Research Area:")) {
            row.innerHTML = `<strong>Research Area:</strong> ${updatedArea}`;
        }
    });
   
    const tagElements = document.querySelectorAll('#editTagContainerElement .tech-tag');
    const techArray = Array.from(tagElements).map(tag => tag.innerText.replace('×', '').trim());

    const dashboardTagContainer = document.querySelector('.readonly-tags');
    if (dashboardTagContainer) {
        dashboardTagContainer.innerHTML = techArray.map(t =>
            `<span class="tech-tag readonly">${t}</span>`
        ).join('');
    }

    closeEditModal();
}
function toggleEditDropdown(event) {
    event.stopPropagation();
    const container = document.getElementById('editResearchDropdown');
    const options = document.getElementById('editDropdownOptions');

    container.classList.toggle('open');
    options.classList.toggle('show');
}

function selectEditResearchArea(id, name) {
    document.getElementById('editSelectedResearchText').innerText = name;
    document.getElementById('hiddenEditResearchAreaId').value = id;

    const container = document.getElementById('editResearchDropdown');
    const options = document.getElementById('editDropdownOptions');
    container.classList.remove('open');
    options.classList.remove('show');
}

window.onclick = function (event) {
    if (!event.target.closest('.custom-dropdown-container')) {
        const options = document.querySelectorAll('.custom-dropdown-options');
        const containers = document.querySelectorAll('.custom-dropdown-container');
        options.forEach(opt => opt.classList.remove('show'));
        containers.forEach(cont => cont.classList.remove('open'));
    }
};

function closeEditModalOutside(event) {
    if (event.target.id === "editProposalModal") {
        closeEditModal();
    }
}

document.getElementById('editFieldTechStack').addEventListener('keyup', function (e) {
    if (e.key === ',' || e.key === 'Enter') {
        let val = this.value.trim();
        if (val.endsWith(',')) {
            val = val.slice(0, -1).trim();
        }
        if (val.length > 0) {
            addTechTag(val);
            this.value = '';
        }
    }
});

function addTechTag(name) {
    const container = document.getElementById('editTagContainerElement');

    const isDuplicate = Array.from(container.querySelectorAll('.tech-tag'))
        .some(tag => tag.innerText.replace('×', '').trim().toLowerCase() === name.toLowerCase());

    if (!isDuplicate) {
        const span = document.createElement('span');
        span.className = 'tech-tag';
        span.innerHTML = `${name} <i onclick="this.parentElement.remove()">&times;</i>`;
        container.appendChild(span);
    }
}
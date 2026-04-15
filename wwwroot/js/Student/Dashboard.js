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

function openEditModal(id, title, areaId, techStack, abstract) {
    document.getElementById('editFieldProjectId').value = id;
    document.getElementById('editFieldProjectTitle').value = title;
    document.getElementById('editFieldAbstract').value = abstract;

    const hiddenInput = document.getElementById('hiddenEditResearchAreaId');
    hiddenInput.value = areaId;

    let areaName = "Select Research Area";
    const dropdownOptions = document.querySelectorAll('#editDropdownOptions li');

    dropdownOptions.forEach(option => {
        const onclickAttr = option.getAttribute('onclick');
        if (onclickAttr && onclickAttr.includes(`'${areaId}'`)) {
            areaName = option.innerText;
        }
    });

    document.getElementById('editSelectedResearchText').innerText = areaName;

    const tagContainer = document.getElementById('editTagContainerElement');
    tagContainer.innerHTML = '';

    if (techStack && techStack !== "undefined" && techStack.trim() !== "") {
        const techs = techStack.split(',');
        techs.forEach(tech => {
            const trimmed = tech.trim();
            if (trimmed) {
                const span = document.createElement('span');
                span.className = 'tech-tag';
                span.innerHTML = `${trimmed} <i onclick="this.parentElement.remove()">&times;</i>`;
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
    const projectId = document.getElementById('editFieldProjectId').value;
    const title = document.getElementById('editFieldProjectTitle').value;
    const abstract = document.getElementById('editFieldAbstract').value;
    const areaId = document.getElementById('hiddenEditResearchAreaId').value;

    const tagElements = document.querySelectorAll('#editTagContainerElement .tech-tag');
    const techStack = Array.from(tagElements)
        .map(tag => tag.innerText.replace('×', '').trim())
        .join(',');

    $.ajax({
        url: '/Students/EditProposal',
        type: 'POST',
        data: {
            id: parseInt(projectId),
            title: title,
            researchAreaId: parseInt(areaId),
            techStack: techStack,
            abstractText: abstract
        },
        success: function (response) {
            if (response.success) {
                location.reload();
            } else {
                alert("Update failed: " + response.message);
            }
        },
        error: function () {
            alert("Error connecting to server.");
        }
    });
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
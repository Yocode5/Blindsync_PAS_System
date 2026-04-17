function openModal() {
    document.getElementById('proposalModal').style.display = 'flex';
}

function closeModal() {
    document.getElementById('proposalModal').style.display = 'none';
}

function closeCreateModalOutside(event) {
    let modal = document.getElementById('proposalModal');
    if (event.target === modal) {
        closeModal();
    }
}

window.onclick = function (event) {
    if (!event.target.closest('.custom-dropdown-container')) {
        const options = document.querySelectorAll('.custom-dropdown-options');
        const containers = document.querySelectorAll('.custom-dropdown-container');
        options.forEach(opt => opt.classList.remove('show'));
        containers.forEach(cont => cont.classList.remove('open'));
    }
};

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
            document.getElementById("techStackError").style.display = "none";
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
    document.getElementById("researchAreaError").style.display = "none";
}

function validateCreateForm() {
    let isValid = true;

    const techInputBox = document.getElementById("techInput");
    if (techInputBox && techInputBox.value.trim() !== '') {
        let tagText = techInputBox.value.trim().replace(',', '');
        if (!techTags.includes(tagText)) {
            techTags.push(tagText);
            renderTags();
        }
        techInputBox.value = '';
    }

    const titleValue = document.getElementById("createTitle").value.trim();
    const titleError = document.getElementById("titleError");
    if (titleValue === "") {
        titleError.style.display = "block";
        isValid = false;
    } else {
        titleError.style.display = "none";
    }

    const researchAreaValue = document.getElementById("hiddenResearchAreaId").value;
    const researchAreaError = document.getElementById("researchAreaError");
    if (researchAreaValue === "") {
        researchAreaError.style.display = "block";
        isValid = false;
    } else {
        researchAreaError.style.display = "none";
    }

    const techStackValue = document.getElementById("hiddenTechStack").value;
    const techStackError = document.getElementById("techStackError");
    if (techStackValue === "") {
        techStackError.style.display = "block";
        isValid = false;
    } else {
        techStackError.style.display = "none";
    }

    const abstractValue = document.getElementById("createAbstract").value.trim();
    const abstractError = document.getElementById("abstractError");
    if (abstractValue === "") {
        abstractError.style.display = "block";
        isValid = false;
    } else {
        abstractError.style.display = "none";
    }

    return isValid;
}

document.getElementById("createTitle").addEventListener("input", function () {
    if (this.value.trim() !== "") document.getElementById("titleError").style.display = "none";
});
document.getElementById("createAbstract").addEventListener("input", function () {
    if (this.value.trim() !== "") document.getElementById("abstractError").style.display = "none";
});

document.addEventListener("DOMContentLoaded", function () {
    let toasts = document.querySelectorAll(".custom-toast");

    toasts.forEach(toast => {
        setTimeout(function () {
            toast.style.animation = "slideOutRight 0.4s ease-in forwards";
            setTimeout(() => toast.remove(), 400);
        }, 3500);
    });
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
    document.querySelectorAll('.error-msg').forEach(el => el.style.display = 'none');

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

function closeEditModalOutside(event) {
    if (event.target.id === "editProposalModal") {
        closeEditModal();
    }
}

function submitEdit() {
    let isValid = true;

    const editTechInput = document.getElementById('editFieldTechStack');
    if (editTechInput && editTechInput.value.trim() !== '') {
        let val = editTechInput.value.trim();
        if (val.endsWith(',')) {
            val = val.slice(0, -1).trim();
        }
        if (val.length > 0) {
            addTechTag(val);
            editTechInput.value = '';
        }
    }

    const projectId = document.getElementById('editFieldProjectId').value;
    const title = document.getElementById('editFieldProjectTitle').value.trim();
    const abstract = document.getElementById('editFieldAbstract').value.trim();
    const areaId = document.getElementById('hiddenEditResearchAreaId').value;

    const tagElements = document.querySelectorAll('#editTagContainerElement .tech-tag');
    const techStack = Array.from(tagElements)
        .map(tag => tag.innerText.replace('×', '').trim())
        .join(',');

    const titleError = document.getElementById("editTitleError");
    if (title === "") {
        titleError.style.display = "block";
        isValid = false;
    } else {
        titleError.style.display = "none";
    }

    const areaError = document.getElementById("editResearchAreaError");
    if (areaId === "") {
        areaError.style.display = "block";
        isValid = false;
    } else {
        areaError.style.display = "none";
    }

    const techError = document.getElementById("editTechStackError");
    if (techStack === "") {
        techError.style.display = "block";
        isValid = false;
    } else {
        techError.style.display = "none";
    }

    const abstractError = document.getElementById("editAbstractError");
    if (abstract === "") {
        abstractError.style.display = "block";
        isValid = false;
    } else {
        abstractError.style.display = "none";
    }

    if (!isValid) {
        return;
    }

    $.ajax({
        url: '/Students/EditProposal',
        type: 'POST',
        data: {
            id: projectId,
            title: title,
            researchAreaId: parseInt(areaId),
            techStack: techStack,
            abstract: abstract
        },
        success: function (response) {
            if (response.success) {
                location.reload();
            } else {
                closeEditModal();
                showCustomErrorToast(response.message);
            }
        },
        error: function () {
            closeEditModal();
            showCustomErrorToast("A network error occurred. Please try again.");
        }
    });
}

document.getElementById("editFieldProjectTitle").addEventListener("input", function () {
    if (this.value.trim() !== "") document.getElementById("editTitleError").style.display = "none";
});
document.getElementById("editFieldAbstract").addEventListener("input", function () {
    if (this.value.trim() !== "") document.getElementById("editAbstractError").style.display = "none";
});

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

    document.getElementById("editResearchAreaError").style.display = "none";
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
            document.getElementById("editTechStackError").style.display = "none";
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

function openViewModal(title, area, tech, abstract) {
    document.getElementById('modalTitle').innerText = title;
    document.getElementById('modalArea').innerText = area;
    document.getElementById('modalAbstractText').innerText = abstract;

    const techContainer = document.getElementById('modalTechStack');
    techContainer.innerHTML = '';

    if (tech) {
        tech.split(',').forEach(t => {
            if (t.trim() !== "") {
                const span = document.createElement('span');
                span.className = 'tech-tag';
                span.innerText = t.trim();
                techContainer.appendChild(span);
            }
        });
    }

    document.getElementById('viewAbstractModal').style.display = 'flex';
}

function closeViewModal() {
    const modal = document.getElementById('viewAbstractModal');
    if (modal) {
        modal.style.display = 'none';
    }
}

function closeViewModalOutside(event) {
    const modal = document.getElementById('viewAbstractModal');
    if (event.target === modal) {
        closeViewModal();
    }
}

function showCustomErrorToast(message) {
    const existingToast = document.getElementById('toastNotificationError');
    if (existingToast) {
        existingToast.remove();
    }

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
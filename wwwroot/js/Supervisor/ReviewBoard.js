document.addEventListener("DOMContentLoaded", function () {
    const selectedTagsContainer = document.getElementById('selectedTags');
    const projectsList = document.getElementById('projectsList');
    const emptyMessage = document.getElementById('emptyMessage');
    const searchInput = document.getElementById('projectSearch');
    const autocompleteList = document.getElementById('autocompleteList');

    let selectedCategories = [];

    let suggestionBank = [];
    document.querySelectorAll('.available-tag').forEach(tag => suggestionBank.push(tag.getAttribute('data-name')));
    document.querySelectorAll('.project-card').forEach(card => suggestionBank.push(card.getAttribute('data-title')));
    suggestionBank = [...new Set(suggestionBank)];

    searchInput.addEventListener('input', function () {
        const val = this.value;
        autocompleteList.innerHTML = '';
        if (!val) {
            autocompleteList.style.display = 'none';
            return;
        }

        let hasMatches = false;
        suggestionBank.forEach(suggestion => {
            if (suggestion.toLowerCase().includes(val.toLowerCase())) {
                hasMatches = true;
                const div = document.createElement('div');
                const regex = new RegExp(`(${val})`, "gi");
                div.innerHTML = suggestion.replace(regex, "<strong>$1</strong>");

                div.addEventListener('click', function () {
                    addCategory(suggestion);
                    searchInput.value = '';
                    autocompleteList.style.display = 'none';
                });
                autocompleteList.appendChild(div);
            }
        });
        autocompleteList.style.display = hasMatches ? 'block' : 'none';
    });

    document.addEventListener("click", function (e) {
        if (e.target !== searchInput) {
            autocompleteList.style.display = 'none';
        }
    });

    document.querySelectorAll('.available-tag').forEach(tag => {
        tag.addEventListener('click', function () {
            addCategory(this.getAttribute('data-name'));
        });
    });

    function addCategory(name) {
        if (!selectedCategories.includes(name)) {
            selectedCategories.push(name);
            const tagSpan = document.createElement('span');
            tagSpan.className = 'expertise-tag';
            tagSpan.setAttribute('data-name', name);
            tagSpan.innerHTML = `${name} <span class="remove-tag" style="cursor:pointer; margin-left:8px; color: #000000; font-weight: bold;">&times;</span>`;
            selectedTagsContainer.appendChild(tagSpan);

            tagSpan.querySelector('.remove-tag').addEventListener('click', () => removeCategory(name));
            updateUI();
        }
    }

    function removeCategory(name) {
        selectedCategories = selectedCategories.filter(cat => cat !== name);
        const tagToRemove = selectedTagsContainer.querySelector(`.expertise-tag[data-name="${name}"]`);
        if (tagToRemove) tagToRemove.remove();
        updateUI();
    }

    function updateUI() {
        const cards = projectsList.querySelectorAll('.project-card');
        let visibleCount = 0;

        cards.forEach(card => {
            if (selectedCategories.length === 0) {
                card.style.display = 'flex';
                visibleCount++;
            } else {
                const title = card.getAttribute('data-title').toLowerCase();
                const area = card.getAttribute('data-area').toLowerCase();

                const isMatch = selectedCategories.some(cat => {
                    const c = cat.toLowerCase();
                    return title.includes(c) || area.includes(c);
                });

                card.style.display = isMatch ? 'flex' : 'none';
                if (isMatch) visibleCount++;
            }
        });

        projectsList.style.display = visibleCount > 0 ? 'flex' : 'none';
        emptyMessage.style.display = visibleCount > 0 ? 'none' : 'block';
    }
    updateUI();
});

function openCustomModal(id, titleEncoded, areaEncoded, techEncoded, abstractEncoded) {
    document.getElementById('selectedProjectId').value = id;
    document.getElementById('modalTitle').innerText = decodeURIComponent(titleEncoded);
    document.getElementById('modalArea').innerText = decodeURIComponent(areaEncoded);
    document.getElementById('modalAbstract').innerText = decodeURIComponent(abstractEncoded);

    const techContainer = document.getElementById('modalTechStack');
    techContainer.innerHTML = '';
    const techStr = decodeURIComponent(techEncoded);

    if (techStr) {
        techStr.split(',').forEach(tech => {
            if (tech.trim() !== "") {
                let span = document.createElement('span');
                span.className = 'tech-tag d-inline-block mb-1';
                span.innerText = tech.trim();

                span.style.backgroundColor = '#2c4a73';
                span.style.color = '#ffffff';
                span.style.padding = '6px 12px';
                span.style.borderRadius = '8px';
                span.style.fontSize = '0.85rem';
                span.style.marginRight = '6px';
                span.style.fontWeight = '500';
                span.style.border = '1px solid rgba(255, 255, 255, 0.1)';

                techContainer.appendChild(span);
            }
        });
    }

    document.getElementById('reviewBoardModalOverlay').style.display = 'flex';
}

function closeCustomModal() {
    document.getElementById('reviewBoardModalOverlay').style.display = 'none';
}

function closeCustomModalOutside(event) {
    const modal = document.getElementById('reviewBoardModalOverlay');
    if (event.target === modal) {
        closeCustomModal();
    }
}

function acceptProject() {
    const projectId = document.getElementById('selectedProjectId').value;
    const acceptBtn = document.querySelector('.btn-accept');

    acceptBtn.disabled = true;
    acceptBtn.innerText = "Accepting...";

    fetch(`/Supervisors/AcceptProject?id=${projectId}`, { method: 'POST' })
        .then(response => {
            if (response.ok) {
                closeCustomModal();
                window.location.href = window.location.pathname;
            } else {
                alert("Failed to accept project. Please try again.");
                acceptBtn.disabled = false;
                acceptBtn.innerText = "Accept Project";
            }
        })
        .catch(error => {
            console.error("Error:", error);
            alert("A network error occurred.");
            acceptBtn.disabled = false;
            acceptBtn.innerText = "Accept Project";
        });
}

function markForReview() {
    const projectId = document.getElementById('selectedProjectId').value;
    const reviewBtn = document.querySelector('.btn-review');

    reviewBtn.disabled = true;
    reviewBtn.innerText = "Marking...";

    fetch(`/Supervisors/MarkForReview?id=${projectId}`, { method: 'POST' })
        .then(response => {
            if (response.ok) {
                closeCustomModal();
                window.location.href = window.location.pathname;
            } else {
                alert("Failed to mark project for review. Please try again.");
                reviewBtn.disabled = false;
                reviewBtn.innerText = "Mark for Review";
            }
        })
        .catch(error => {
            console.error("Error:", error);
            alert("A network error occurred.");
            reviewBtn.disabled = false;
            reviewBtn.innerText = "Mark for Review";
        });
}

document.addEventListener("DOMContentLoaded", function () {
    const toasts = document.querySelectorAll('.custom-toast');

    toasts.forEach(toast => {
        setTimeout(() => {
            toast.classList.add('slide-out');
            setTimeout(() => {
                toast.remove();
            }, 400);

        }, 3000);
    });
});
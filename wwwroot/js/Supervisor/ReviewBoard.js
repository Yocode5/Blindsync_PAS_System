function openModal(id, title, area, techStackStr, abstractText) {
    let doc = new DOMParser().parseFromString(title, "text/html");
    let decodedTitle = doc.documentElement.textContent;
    
    let docAbs = new DOMParser().parseFromString(abstractText, "text/html");
    let decodedAbstract = docAbs.documentElement.textContent;

    document.getElementById('selectedProjectId').value = id;
    document.getElementById('modalTitle').innerText = decodedTitle;
    document.getElementById('modalArea').innerText = area;
    document.getElementById('modalAbstract').innerText = decodedAbstract;

    let techContainer = document.getElementById('modalTechStack');
    techContainer.innerHTML = '';
    let techs = techStackStr.split(',');
    
    techs.forEach(tech => {
        if(tech.trim() !== "") {
            let span = document.createElement('span');
            span.className = 'tech-tag d-inline-block mb-1';
            span.innerText = tech.trim();
            span.style.backgroundColor = '#20B2AA'; 
            span.style.color = 'black';
            techContainer.appendChild(span);
        }
    });    
    var myModal = new bootstrap.Modal(document.getElementById('projectModal'));
    myModal.show();
}

function acceptProject() {
    const projectId = document.getElementById('selectedProjectId').value;
    fetch(`/Supervisors/AcceptProject?id=${projectId}`, { method: 'POST' })
    .then(response => {
        if (response.ok) {
            alert("Project accepted successfully!");
            location.reload();
        }
    });
}

// Logic for dynamic filtering
document.addEventListener("DOMContentLoaded", function() {
    const selectedTagsContainer = document.getElementById('selectedTags');
    const availableTagsContainer = document.getElementById('availableTags');
    const projectsList = document.getElementById('projectsList');
    const emptyMessage = document.getElementById('emptyMessage');
    const searchInput = document.getElementById('projectSearch');
    let selectedCategories = [];

    const initialSelectedTags = selectedTagsContainer.querySelectorAll('.expertise-tag');
    initialSelectedTags.forEach(tag => {
        const name = tag.getAttribute('data-name');
        selectedCategories.push(name);
        tag.querySelector('.remove-tag').addEventListener('click', () => removeCategory(name));
    });

    document.querySelectorAll('.available-tag').forEach(tag => {
        const name = tag.getAttribute('data-name');
        tag.addEventListener('click', () => addCategory(name));
    });

    function addCategory(name) {
        if (!selectedCategories.includes(name)) {
            selectedCategories.push(name);
            const tagSpan = document.createElement('span');
            tagSpan.className = 'expertise-tag';
            tagSpan.setAttribute('data-name', name);
            tagSpan.innerHTML = `${name} <span class="remove-tag">&times;</span>`;
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
        document.querySelectorAll('.available-tag').forEach(tag => {
            tag.style.display = selectedCategories.includes(tag.getAttribute('data-name')) ? 'none' : 'block';
        });

        const cards = projectsList.querySelectorAll('.project-card');
        if (selectedCategories.length === 0) {
            cards.forEach(card => card.style.display = 'none');
            emptyMessage.style.display = 'block';
            projectsList.style.display = 'none';
        } else {
            projectsList.style.display = 'flex';
            emptyMessage.style.display = 'none';
            cards.forEach(card => {
                card.style.display = selectedCategories.includes(card.getAttribute('data-area')) ? 'flex' : 'none';
            });
        }
        filterSearch();
    }
    function filterSearch() {
        let query = searchInput.value.toLowerCase();
        projectsList.querySelectorAll('.project-card').forEach(card => {
            if (selectedCategories.includes(card.getAttribute('data-area'))) {
                const title = card.getAttribute('data-title');
                const area = card.getAttribute('data-area').toLowerCase();
                const match = title.includes(query) || area.includes(query);
                card.style.display = match ? 'flex' : 'none';
            }
        });
    }
    searchInput.addEventListener('keyup', filterSearch);
    updateUI();
});
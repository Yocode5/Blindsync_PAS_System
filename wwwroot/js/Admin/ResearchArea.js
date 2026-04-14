// wwwroot/js/Admin/ResearchArea.js

function addNewArea() {
    const input = document.getElementById('newAreaInput');
    const areaName = input.value.trim();

    if (areaName === "") {
        alert("Please enter a research area!");
        return;
    }
    const container = document.getElementById('areasContainer');
    const newTag = document.createElement('div');
    newTag.className = 'area-tag';
    newTag.innerHTML = `
        <span>${areaName}</span>
        <i class="fas fa-times btn-delete-tag" onclick="this.parentElement.remove()"></i>
    `;

    container.appendChild(newTag);
    input.value = ""; 
}
function deleteArea(id) {
    if (confirm("Are you sure you want to delete this research area?")) {
        const element = document.getElementById(`area-${id}`);
        if (element) {
            element.remove();
        }
    }
}
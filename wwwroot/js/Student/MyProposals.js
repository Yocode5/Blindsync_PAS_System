function openViewModal(title, area, tech, abstract) {
    console.log("Opening modal for:", title);

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
    console.log("Close button clicked");
    const modal = document.getElementById('viewAbstractModal');
    if (modal) {
        modal.style.display = 'none';
    }
}
function closeViewModalOutside(event) {
    const modal = document.getElementById('viewAbstractModal');
   
    if (event.target === modal) {
        console.log("Clicked outside the modal container");
        closeViewModal();
    }
}
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
                
                span.innerHTML = `<span>${t.trim()}</span>`;
                techContainer.appendChild(span);
            }
        });
    }

    
    document.getElementById('viewAbstractModal').style.display = 'flex';
}

function closeViewModal() {
    document.getElementById('viewAbstractModal').style.display = 'none';
}


function closeViewModalOutside(event) {
    const overlay = document.getElementById('viewAbstractModal');
    if (event.target === overlay) {
        closeViewModal();
    }
}
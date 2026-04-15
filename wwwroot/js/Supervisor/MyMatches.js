function openProposalModal(title, area, tech, abstract) {
    document.getElementById('modalTitle').innerText = title;
    document.getElementById('modalArea').innerText = area;
    document.getElementById('modalAbstractText').innerText = abstract;

    const techContainer = document.getElementById('modalTechStack');
    techContainer.innerHTML = '';

    if (tech) {
        tech.split(',').forEach(t => {
            if (t.trim() !== "") {
                const span = document.createElement('span');
                span.className = 'modal-tech-pill';
                span.innerText = t.trim();
                techContainer.appendChild(span);
            }
        });
    }

    document.getElementById('proposalModalOverlay').style.display = 'flex';
}

function closeProposalModal() {
    document.getElementById('proposalModalOverlay').style.display = 'none';
}

function closeProposalModalOutside(event) {
    const modal = document.getElementById('proposalModalOverlay');
    if (event.target === modal) {
        closeProposalModal();
    }
}
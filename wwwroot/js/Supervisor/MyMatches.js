function openProposalModal() {
    document.getElementById('proposalModalOverlay').classList.add('active');
}

function closeProposalModal() {
    document.getElementById('proposalModalOverlay').classList.remove('active');
}

document.getElementById('proposalModalOverlay').addEventListener('click', function (e) {
    if (e.target === this) {
        closeProposalModal();
    }
});
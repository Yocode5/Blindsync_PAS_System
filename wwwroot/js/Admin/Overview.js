function openReassignModal(studentName, projectName, currentSupervisor) {
    document.getElementById('modalStudentName').innerText = studentName;
    document.getElementById('modalProjectName').innerText = projectName;
    document.getElementById('modalCurrentSupervisor').innerText = currentSupervisor;
    document.getElementById('reassignModal').style.display = 'flex';
}
function closeReassignModal() {
    document.getElementById('reassignModal').style.display = 'none';
    document.getElementById('newSupervisorSelect').value = "";
}
function submitReassign() {
    const newSupervisor = document.getElementById('newSupervisorSelect').value;
    if(newSupervisor === "") {
        alert("Please select a new supervisor from the list.");
        return;
    }
    alert(`Success! Project reassigned to ${newSupervisor}.`);
    closeReassignModal();
}
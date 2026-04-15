let currentProjectId = null;

function openReassignModal(projectId, studentName, projectName, currentSupervisor) {
    currentProjectId = projectId; // ආපු ID එක save කරගන්නවා
    document.getElementById('modalStudentName').innerText = studentName;
    document.getElementById('modalProjectName').innerText = projectName;
    document.getElementById('modalCurrentSupervisor').innerText = currentSupervisor;
    document.getElementById('reassignModal').style.display = 'flex';
}

function closeReassignModal() {
    document.getElementById('reassignModal').style.display = 'none';
    document.getElementById('newSupervisorSelect').value = "";
    currentProjectId = null; 
}

async function submitReassign() {
    const newSupervisorId = document.getElementById('newSupervisorSelect').value;
    
    if(newSupervisorId === "") {
        alert("Please select a new supervisor from the list.");
        return;
    }
    
    try {
        const response = await fetch('/Admin/ReassignSupervisor', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },          
            body: JSON.stringify({ 
                ProjectId: parseInt(currentProjectId), 
                SupervisorId: parseInt(newSupervisorId) 
            })
        });
        const result = await response.json();

        if (result.success) {          
            closeReassignModal();
            location.reload(); 
        } else {
            alert("Error: " + result.message);
        }
    } catch (error) {
        console.error("Error:", error);
        alert("Something went wrong. Please try again.");
    }
}
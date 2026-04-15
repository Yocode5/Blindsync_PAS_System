async function addNewArea() {
    //Reference the input field and get its trimmed value
    const input = document.getElementById('newAreaInput');
    const areaName = input.value.trim();
    //Client-side validation to prevent empty submissions
    if (areaName === "") {
        alert("Please enter a research area name!");
        return;
    }

    try {       
        const response = await fetch('/Admin/AddResearchArea', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ Name: areaName })
        });
        const result = await response.json();
        if (result.success) {            
            input.value = "";            
            location.reload(); 
        } else {           
            alert(result.message);
        }
    } catch (error) {
        console.error("Error adding research area:", error);
        alert("Something went wrong. Please check your connection.");
    }
}

/**

 * @param {number} id - The unique Database ID of the Research Area.
 */
async function deleteArea(id) {    
    if (!confirm("Are you sure you want to delete this research area?")) return;
    try {       
        const response = await fetch(`/Admin/DeleteResearchArea?id=${id}`, {
            method: 'POST'
        });
        const result = await response.json();        
        if (result.success) {           
            const tag = document.getElementById(`area-${id}`);            
            if (tag) {                
                tag.style.transition = 'opacity 0.3s ease';
                tag.style.opacity = '0';                                
                setTimeout(() => tag.remove(), 300);
            }
        } else {            
            alert(result.message);
        }
    } catch (error) {
        console.error("Error deleting research area:", error);
        alert("An error occurred while deleting. Please try again.");
    }
}
/**
 * BlindSync Dashboard - Main Application Logic
 * Handles: Search, Filters, Modal, Project Interactions
 */

// -------------------------------
// PROJECT DATA (in production, this comes from server)
// -------------------------------
let projectsData = {
    1: {
        name: "AI-Powered Code Assistant",
        researchArea: "Artificial Intelligence",
        techStack: ["Python", "TensorFlow"],
        abstract: "This research explores integrating large language models into developer tooling to assist with real-time code generation, debugging, and documentation."
    },
    2: {
        name: "Decentralized Identity Management",
        researchArea: "Web Development",
        techStack: ["React", "Solidity"],
        abstract: "A blockchain-based identity framework that gives users control over personal data using zero-knowledge proofs."
    },
    3: {
        name: "Zero-Trust Network Security",
        researchArea: "Cybersecurity",
        techStack: ["Go", "Kubernetes"],
        abstract: "Implementation of micro-segmentation and continuous verification for cloud-native environments."
    }
};

let currentProjectId = null;

// DOM Elements
const searchInput = document.getElementById('searchInput');
const filterButtonsContainer = document.getElementById('filterButtonsContainer');
const projectsTitle = document.getElementById('projectsTitle');
const projectsGrid = document.getElementById('projectsGrid');

// -------------------------------
// INITIALIZATION - Hide sections until search
// -------------------------------
filterButtonsContainer.style.display = 'none';
projectsTitle.style.display = 'none';
projectsGrid.style.display = 'none';

// Render project cards on page load
renderProjectCards();

// -------------------------------
// RENDER PROJECT CARDS
// -------------------------------
function renderProjectCards() {
    let cardsHtml = '';
    
    for (const [id, project] of Object.entries(projectsData)) {
        // Map research area to data-research attribute for filtering
        let researchKey = 'AI';
        if (project.researchArea === 'Web Development') researchKey = 'Web';
        if (project.researchArea === 'Cybersecurity') researchKey = 'Cyber';
        
        // Build tech stack badges
        let techBadges = '';
        project.techStack.forEach(tech => {
            techBadges += `<span class="tech-tag">${escapeHtml(tech)}</span>`;
        });
        
        cardsHtml += `
            <div class="project-card" data-research="${researchKey}" data-id="${id}">
                <div class="project-name">${escapeHtml(project.name)}</div>
                <div class="project-details">
                    <div class="research-row">
                        <span class="research-label">Research Area:</span>
                        <span class="research-value">${escapeHtml(project.researchArea)}</span>
                    </div>
                    <div class="tech-row">
                        <span class="tech-label">Tech Stack:</span>
                        <div class="tech-stack">${techBadges}</div>
                    </div>
                </div>
                <button class="view-more-btn" onclick="viewProjectDetails(${id})">View More Details</button>
            </div>
        `;
    }
    
    projectsGrid.innerHTML = cardsHtml;
}

// Helper to escape HTML
function escapeHtml(str) {
    if (!str) return '';
    return str.replace(/&/g, '&amp;')
              .replace(/</g, '&lt;')
              .replace(/>/g, '&gt;')
              .replace(/"/g, '&quot;')
              .replace(/'/g, '&#39;');
}

// -------------------------------
// SEARCH FUNCTIONALITY
// -------------------------------
if (searchInput) {
    searchInput.addEventListener('input', function(e) {
        const term = e.target.value.toLowerCase().trim();
        
        // Show/hide filter section based on search input
        if (term !== '') {
            filterButtonsContainer.style.display = 'flex';
            projectsTitle.style.display = 'block';
            projectsGrid.style.display = 'flex';
        } else {
            filterButtonsContainer.style.display = 'none';
            projectsTitle.style.display = 'none';
            projectsGrid.style.display = 'none';
        }
        
        // Filter project cards
        const projectCards = document.querySelectorAll('.project-card');
        let visibleCount = 0;
        
        projectCards.forEach(card => {
            const cardText = card.textContent.toLowerCase();
            if (cardText.includes(term)) {
                card.style.display = 'flex';
                visibleCount++;
            } else {
                card.style.display = 'none';
            }
        });
        
        // Show "no results" message if needed
        let noResultsMsg = document.getElementById('noResultsMsg');
        if (visibleCount === 0 && term !== '') {
            if (!noResultsMsg) {
                noResultsMsg = document.createElement('div');
                noResultsMsg.id = 'noResultsMsg';
                noResultsMsg.style.cssText = 'color: white; text-align: center; padding: 40px; font-size: 18px; width: 100%;';
                noResultsMsg.textContent = 'No projects found matching "' + term + '"';
                projectsGrid.appendChild(noResultsMsg);
            }
        } else {
            if (noResultsMsg) noResultsMsg.remove();
        }
    });
}

// -------------------------------
// FILTER BUTTONS LOGIC
// -------------------------------
document.querySelectorAll('.filter-btn').forEach(btn => {
    btn.addEventListener('click', function(e) {
        // Don't trigger if clicking the close X
        if (e.target.classList.contains('close-x')) return;
        
        // Update active state
        document.querySelectorAll('.filter-btn').forEach(b => b.classList.remove('active'));
        this.classList.add('active');
        
        // Filter projects
        const expertise = this.getAttribute('data-expertise');
        document.querySelectorAll('.project-card').forEach(card => {
            if (expertise === 'all' || card.getAttribute('data-research') === expertise) {
                card.style.display = 'flex';
            } else {
                card.style.display = 'none';
            }
        });
    });
});

// Remove individual filter button
function removeFilter(el) {
    el.parentElement.style.display = 'none';
    showToast('Filter removed', true);
}

// -------------------------------
// MODAL - VIEW MORE DETAILS
// -------------------------------
function viewProjectDetails(projectId) {
    currentProjectId = projectId;
    const project = projectsData[projectId];
    
    // Build tech stack badges for modal
    const techBadgesHtml = project.techStack.map(tech => 
        `<span class="modal-tech-tag">${escapeHtml(tech)}</span>`
    ).join('');
    
    // Modal body HTML
    const modalBodyHtml = `
        <div class="modal-project-title">${escapeHtml(project.name)}</div>
        
        <div class="modal-research-row">
            <div class="modal-research-label">Research Area:</div>
            <div class="modal-research-value">${escapeHtml(project.researchArea)}</div>
        </div>
        
        <div class="modal-tech-section">
            <div class="modal-tech-label">Tech Stack:</div>
            <div class="modal-tech-stack">${techBadgesHtml}</div>
        </div>
        
        <div class="abstract-section">
            <div class="abstract-label">Abstract:</div>
            <textarea id="abstractText" class="abstract-textarea" placeholder="Write or edit abstract here...">${escapeHtml(project.abstract)}</textarea>
        </div>
    `;
    
    // Button HTML
    const buttonsHtml = `
        <button class="accept-btn" id="acceptBtn">Accept Project</button>
        <button class="cancel-modal-btn" id="cancelModalBtn">Cancel</button>
    `;
    
    // Inject into modal
    document.getElementById('modalBody').innerHTML = modalBodyHtml;
    document.getElementById('modalButtonGroup').innerHTML = buttonsHtml;
    
    // Attach button events
    const acceptBtn = document.getElementById('acceptBtn');
    const cancelBtn = document.getElementById('cancelModalBtn');
    
    if (acceptBtn) {
        acceptBtn.addEventListener('click', function() {
            const editedAbstract = document.getElementById('abstractText').value;
            projectsData[currentProjectId].abstract = editedAbstract;
            showToast(`✓ Interest expressed for "${projectsData[currentProjectId].name}"`, true);
            closeModal();
        });
    }
    
    if (cancelBtn) {
        cancelBtn.addEventListener('click', closeModal);
    }
    
    // Show modal
    document.getElementById('detailsModal').style.display = 'block';
}

// Close modal
function closeModal() {
    document.getElementById('detailsModal').style.display = 'none';
    currentProjectId = null;
}

// TOAST NOTIFICATION
function showToast(message, isSuccess = true) {
    const toast = document.getElementById('toast');
    toast.textContent = message;
    toast.style.backgroundColor = isSuccess ? '#10b981' : '#E27272';
    toast.style.display = 'block';
    
    setTimeout(() => {
        toast.style.display = 'none';
    }, 3000);
}

// Close modal when clicking outside
window.onclick = function(event) {
    const modal = document.getElementById('detailsModal');
    if (event.target === modal) closeModal();
};
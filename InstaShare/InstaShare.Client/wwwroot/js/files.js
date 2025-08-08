let allFiles = [];
let currentPage = 1;
const pageSize = 5;

async function fetchFiles() {
    const apiResult = await fetch(`${baseUrl}/api/v1/Files/getFiles`);

    if (!apiResult.ok)
        throw new Error("Failed to fetch offices");

    const allFiles = await apiResult.json();

    console.log("Raw Api results" + allFiles);

    const offices = result.data;

    console.log("Files: " + JSON.stringify(allFiles));

    renderPage();
}

function renderPage() {
    const start = (currentPage - 1) * pageSize;
    const end = start + pageSize;
    const files = allFiles.slice(start, end);

    const container = document.getElementById('filesContainer');
    container.innerHTML = files.map(f => `
            <div class="file-row">
                <strong>${f.name}</strong> - ${f.size} bytes - ${f.status}
                <button onclick="renameFile('${f.id}')">Rename</button>
            </div>
            `).join('');

    // Pagination buttons
    document.getElementById('prevBtn').disabled = currentPage === 1;
    document.getElementById('nextBtn').disabled = end >= allFiles.length;
    document.getElementById('pageIndicator').textContent = `Page ${currentPage}`;
}

document.getElementById('prevBtn').addEventListener('click', () => {
    if (currentPage > 1) {
        currentPage--;
        renderPage();
    }
});

document.getElementById('nextBtn').addEventListener('click', () => {
    if (currentPage * pageSize < allFiles.length) {
        currentPage++;
        renderPage();
    }
});

function renameFile(id) {
    const newName = prompt("Enter new name:");
    if (newName) {
        fetch(`/Files/Rename?id=${id}&newName=${encodeURIComponent(newName)}`, {
            method: 'POST'
        }).then(fetchFiles);
    }
}

fetchFiles();

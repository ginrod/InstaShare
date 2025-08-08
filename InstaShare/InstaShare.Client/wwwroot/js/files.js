let allFiles = [];
let page = 1;
const pageSize = 5;

async function searchFiles() {
    const searchFilesInput = document.getElementById("searchFilesInput");
    const baseUrl = searchFilesInput.dataset.api;
    const term = searchFilesInput.value;

    const apiResult = await fetch(`${baseUrl}/api/v1/Files/searchFiles?term=${term}&page=${page}`);

    if (!apiResult.ok)
        throw new Error("Failed to fetch files");

    const jsonResult = await apiResult.json();
    allFiles = jsonResult.data

    console.log("Raw Api results" + allFiles);

    console.log("Files: " + JSON.stringify(allFiles));

    renderPage();
}

async function findAllFiles() {
    const searchFilesInput = document.getElementById("searchFilesInput");
    const baseUrl = searchFilesInput.dataset.api;

    const apiResult = await fetch(`${baseUrl}/api/v1/Files/getAllFiles?page=${page}`);

    if (!apiResult.ok)
        throw new Error("Failed to fetch files");

    const jsonResult = await apiResult.json();
    allFiles = jsonResult.data

    console.log("Raw Api results" + allFiles);

    console.log("Files: " + JSON.stringify(allFiles));

    renderPage();
}

function renderPage() {
    const start = (page - 1) * pageSize;
    const end = start + pageSize;
    const files = allFiles.slice(start, end);

    // Pagination buttons
    document.getElementById('prevBtn').disabled = page  === 1;
    document.getElementById('nextBtn').disabled = end >= allFiles.length;
    document.getElementById('pageIndicator').textContent = `Page ${page}`;

    renderTable(files);
}

function renderTable(files) {
    const filesContainerDiv = document.getElementById("filesContainer");

    if (!files || files.length === 0) {
        filesContainerDiv.textContent = "No files found.";

        document.getElementById("paginationControls").style.display = "none";
    }

    const table = document.createElement("table");
    table.style.borderCollapse = "collapse";
    table.classList.add("table", "table-bordered", "table-striped", "w-100");

    // Header row
    const header = document.createElement("tr");
    const columnNames = ["Name", "Status", "Size"];

    columnNames.forEach(text => {
        const th = document.createElement("th");
        th.textContent = text;

        th.style.border = "1px solid #ccc";
        th.style.padding = "8px";
        header.appendChild(th)
    });

    table.appendChild(header);

    // Data rows
    files.forEach(c => {
        const tr = document.createElement("tr");

        const contactDetails = [c.name, c.status, c.size];

        contactDetails.forEach(detail => {
            const td = document.createElement("td");

            td.textContent = detail;
            td.style.border = "1px solid #ccc";
            td.style.padding = "8px";
            tr.appendChild(td);
        });
        table.appendChild(tr);
    });

    filesContainerDiv.innerHTML = "";
    filesContainerDiv.appendChild(table);
}

function nextPage() {
    if (page < totalPages) {
        ++page;
        searchContacts();
    }
}

function previousPage() {
    if (page > 1) {
        --page;
        searchContacts();
    }
}

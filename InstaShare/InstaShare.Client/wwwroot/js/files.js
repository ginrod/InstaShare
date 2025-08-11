let allFiles = [];
let page = 1;
const pageSize = 5;
let totalPages = 1;

async function uploadFile(term, page) {
    page = page || 1;

    const baseUrl = window.appConfig.baseUrl;
    const token = window.appConfig.accessToken;

    console.log("Token: " + token)

    const apiResult = await fetch(`${baseUrl}/api/v1/Files/uploadFile`, {
        headers: {
            "Authorization": `Bearer ${token}`,
            "Accept": "application/json"
        }
    });

    if (!apiResult.ok)
        throw new Error("Failed to upload file");
}

async function searchFiles(page) {

    const searchFilesInput = document.getElementById("searchFilesInput");

    const term = searchFilesInput.value;

    if (term) {
        await searchByTermFiles(term, page);
    }
    else
    {
        await searchAllFiles(page);
    }
}

async function searchByTermFiles(term, page) {
    page = page || 1;

    const baseUrl = window.appConfig.baseUrl;
    const token = window.appConfig.accessToken;

    console.log("Token: " + token)

    const apiResult = await fetch(`${baseUrl}/api/v1/Files/searchFiles?term=${encodeURIComponent(term)}&page=${page}`, {
        headers: {
            "Authorization": `Bearer ${token}`,
            "Accept": "application/json"
        }
    });

    if (!apiResult.ok)
        throw new Error("Failed to fetch files");

    const jsonResult = await apiResult.json();
    const files = jsonResult.data
    totalPages = Math.ceil(jsonResult.totalRecords / pageSize);

    console.log("Raw Api results" + files);

    console.log("Files: " + JSON.stringify(files));

    renderPage(files, totalPages);
}

async function searchAllFiles(page) {
    page = page || 1;

    const baseUrl = window.appConfig.baseUrl;
    const token = window.appConfig.accessToken;

    console.log("Token: " + token)

    const apiResult = await fetch(`${baseUrl}/api/v1/Files/getAllFiles?page=${page}`, {
        headers: {
            "Authorization": `Bearer ${token}`,
            "Accept": "application/json"
        }
    });

    if (!apiResult.ok)
        throw new Error("Failed to fetch files");

    const jsonResult = await apiResult.json();
    const files = jsonResult.data
    totalPages = Math.ceil(jsonResult.totalRecords / pageSize);

    console.log("Raw Api results" + files);
    console.log("JsonResult.totalRecord=" + jsonResult.totalRecords);

    console.log("Files: " + JSON.stringify(files));

    renderPage(files, totalPages);
}

function renderPage(files, totalPages) {

    renderTable(files);

    document.getElementById("paginationControls").style.display = "block";
    document.getElementById("pageIndicator").textContent = page;
    document.getElementById("totalPages").textContent = totalPages;
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
        searchFiles(page);
    }
}

function previousPage() {
    if (page > 1) {
        --page;
        searchFiles(page);
    }
}

function formatSize(bytes) {
    const units = ['B', 'KB', 'MB', 'GB'];
    let i = 0, n = bytes;
    while (n >= 1024 && i < units.length - 1) { n /= 1024; i++; }
    return `${n.toFixed(1)} ${units[i]}`;
}

let allFiles = [];
let page = 1;
const pageSize = 5;
let totalPages = 1;

async function searchFiles(page) {

    const searchFilesInput = document.getElementById("searchFilesInput");
    const baseUrl = searchFilesInput.dataset.api;
    const term = searchFilesInput.value;

    if (term) {
        await searchByTermFiles(baseUrl, term, page);
    }
    else
    {
        await searchAllFiles(page);
    }
}

async function searchByTermFiles(baseUrl, term, page) {
    page = page || 1;

    const token = "@Model.AccessToken"

    const headers = new Headers({
        "Authorization": `Bearer ${token}`
    });

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

    const searchFilesInput = document.getElementById("searchFilesInput");
    baseUrl = searchFilesInput.dataset.api;

    const apiResult = await fetch(`${baseUrl}/api/v1/Files/getAllFiles?page=${page}`);

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

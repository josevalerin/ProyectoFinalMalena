function sortTableByColum(table, colum, asc = true) {
    const dirModifer = asc ? 1 : -1;
    const tbody = table.tBodies[0];
    const rows = Array.from(tbody.querySelectorAll('tr'));
    const sortedRows = rows.sort((a, b) => {
        const aColtext = a.querySelector(`td:nth-child(${colum + 1})`).textContent.trim();
        const bColtext = b.querySelector(`td:nth-child(${colum + 1})`).textContent.trim();
        return aColtext > bColtext ? (1 * dirModifer) : (-1 * dirModifer);
    });

    while (tbody.firstChild) {
        tbody.removeChild(tbody.firstChild);
    }
    tbody.append(...sortedRows);

    table.querySelectorAll("th").forEach(th => th.classList.remove('th-sort-asc', 'th-sort-desc'));
    table.querySelector(`th:nth-child(${colum + 1})`).classList.toggle('th-sort-asc', asc);
    table.querySelector(`th:nth-child(${colum + 1})`).classList.toggle('th-sort-desc', !asc);

}

document.querySelectorAll(".tabla-sortable th").forEach(headerCell => {
    headerCell.addEventListener("click", () => {
        const tableElement = headerCell.parentElement.parentElement.parentElement;
        const headerIndex = Array.prototype.indexOf.call(headerCell.parentElement.children, headerCell);
        const currentIsAscending = headerCell.classList.contains("th-sort-asc");
        sortTableByColum(tableElement, headerIndex, !currentIsAscending);
    })
});


const searchInput = document.getElementById("search-input");
searchInput.addEventListener("keyup", function () {
    const trs = document.querySelectorAll(".tabla-search tr:not(.header-tabla)");
    const filter = document.querySelector("#search-input").value;
    const regex = new RegExp(filter, "i");
    const isFoundInTds = (td) => regex.test(td.innerHTML);
    const isFound = (childrenArr) => childrenArr.some(isFoundInTds);
    const setTrStyleDisplay = ({ style, children }) => {
        style.display = isFound([...children]) ? "" : "none";
    };

    trs.forEach(setTrStyleDisplay);
});
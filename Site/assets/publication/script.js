const { platform } = require("os");
const { monitorEventLoopDelay } = require("perf_hooks");

function showTab(tabId) {
    var tabContent = document.getElementById(tabId);

    const tabsToHide = document.getElementsByClassName('tab-content');

    Array.from(tabsToHide).forEach(element => {
        element.style.display = "none";
    });

    tabContent.style.display = "block";


}


function scaleDeleteButton(id) {
    document.getElementById("btnDelete" + id).style.transform = "scale(1.2,1.2)";
}
function scaleEditButton(id) {
    document.getElementById("btnEdit" + id).style.transform = "scale(1.2,1.2)";
}
function scaleDeleteButtonBack(id) {
    document.getElementById("btnDelete" + id).style.transform = "scale(1,1)";
}
function scaleEditButtonBack(id) {
    document.getElementById("btnEdit" + id).style.transform = "scale(1,1)";
}






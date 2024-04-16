document.addEventListener('DOMContentLoaded', function () {
    const isFirstTime = sessionStorage.getItem('isFirstTime');
    if (!isFirstTime) {
        const filePath = CollectData();
        sessionStorage.setItem('isFirstTime', 'true');
    } else {
        // If the request has already been made, hide the loading content
        const storedFilePath = sessionStorage.getItem('filePath');
        if (storedFilePath) {
            // Use the storedFilePath as needed
            console.log("Stored filePath:", storedFilePath);
        } else {
            console.log("filePath is not stored in sessionStorage.");
        }
        document.querySelector('.loading-content').style.display = 'none';
        const fileName = document.getElementById('fileName_input').value;

        const downloadContent = document.getElementById('Download-content');
        /* downloadContent.style.display = 'contents'; */
        const downloadLink = document.createElement('a');
        downloadLink.id = 'download_btnn';
        downloadLink.href = storedFilePath;
        downloadLink.download = fileName;
        downloadLink.textContent = 'Download File';
        // Append the <a> element to Download-content
        downloadContent.appendChild(downloadLink);
        downloadContent.style.display = 'contents';
    }
});

 function CollectData() {
    // Take value of input hidden field
    const fileName = document.getElementById('fileName_input').value;
    const sessionID = document.getElementById('sessionID_input').value;
    const filePath = "https://localhost:7125/Download/MockExam_" + sessionID + ".pdf";

    try {
        const response = $.ajax({
            type: "POST",
            url: "https://localhost:7125/UploadFile_AI_Module_API/" + fileName + "/" + sessionID,
            dataType: "json",
        });

        console.log(response);

        sessionStorage.setItem('filePath', filePath);

        return filePath;
    } catch (error) {
        console.log("Error occurred:", error);
    }
}

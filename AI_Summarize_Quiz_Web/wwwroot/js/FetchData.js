document.addEventListener('DOMContentLoaded', function () {
    const isFirstTime = sessionStorage.getItem('isFirstTime');
    if (!isFirstTime) {
        CollectData();
        sessionStorage.setItem('isFirstTime', 'true');
    } else {
        // If the request has already been made, hide the loading content
        document.querySelector('.loading-content').style.display = 'none';
        var downloadContent = document.getElementById('Download-content');

        var fileName = document.getElementById('fileName_input').value;
        var sessionID = document.getElementById('sessionID_input').value;
        var filePath = "https://localhost:7125/Download/MockExam_" +  sessionID + ".pdf";
        downloadContent.style.display = 'contents';
        var downloadLink = document.createElement('a');
        downloadLink.id = 'download_btnn';
        downloadLink.href = filePath;
        downloadLink.download = fileName;
        downloadLink.textContent = 'Download File';

        // Append the <a> element to Download-content
        downloadContent.appendChild(downloadLink);
    }
});
async function CollectData() {
    // Take value of input hidden field
    var fileName = document.getElementById('fileName_input').value;
    var sessionID = document.getElementById('sessionID_input').value;

    $.ajax({
        type: "GET",
        url: "https://localhost:7125/UploadFile_AI_Module/" + fileName + "/" + sessionID,
        dataType: "json", // Set the data type to blob to handle binary data
        success: function (response) {
            console.log(response);

            if (response && response.SessionId) {
                console.log("SessionId retrieved successfully:", response.SessionId);

                // Hide loading content
                document.querySelector('.loading-content').style.display = 'none';

                // Display download content
                var downloadContent = document.getElementById('Download-content');
                downloadContent.style.display = 'contents';
            } else {
                console.log("SessionId not found in the response.");
            }
        },

        error: function (error) {
            console.log("Error occurred:", error);
        }
    });
}

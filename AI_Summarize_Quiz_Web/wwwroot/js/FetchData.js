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
        const box_download = document.getElementById('Download_content_box_outside');
        box_download.style.textAlign = 'center';
        /* downloadContent.style.display = 'contents'; */
        const downloadLink = document.createElement('a');
        downloadLink.id = 'download_btnn';
        downloadLink.href = storedFilePath;
        downloadLink.download = fileName;
        downloadLink.textContent = 'Download File';
        // Append the <a> element to Download-content
        downloadContent.appendChild(downloadLink);
        box_download.style.display = 'contents';
        downloadContent.style.display = 'contents';

        // Check if there's a stored file path
        //if (storedFilePath) {
        //    const htmlFilePath = storedFilePath.replace(/\.pdf$/, ".txt");
        //    // Read PDF file from stored file path


        //    var iframe = document.createElement('iframe');
        //    iframe.src = htmlFilePath;
        //    iframe.width = "100%";
        //    iframe.height = "100%";
        //    iframe.style.border = "none";

        //    document.getElementById('fileContent').appendChild(iframe);

        //} else {
            
        //}
    }
});

 function CollectData() {
    // Take value of input hidden field
    const fileName = document.getElementById('fileName_input').value;
    const sessionID = document.getElementById('sessionID_input').value;
    const filePath = "https://localhost:7125/Download/MockExam_" + sessionID + ".pdf";

    try {
        $.ajax({
            type: "POST",
            url: "https://localhost:7125/UploadFile_AI_Module_API/" + fileName + "/" + sessionID,
            dataType: "json",

            success: function (data) {
                console.log(data);
            },

            error: function (error) {
                console.log(error);
            }
        });

        //console.log(response);

        sessionStorage.setItem('filePath', filePath);

        return filePath;
    } catch (error) {
        console.log("Error occurred:", error);
    }
}

function ReturnButton() {
    sessionStorage.clear();
    window.location.href = "https://localhost:7125/";
}

document.addEventListener('DOMContentLoaded', function () {
    const isFirstTime = sessionStorage.getItem('isFirstTime');
    if (!isFirstTime) {
        FetchData();
        sessionStorage.setItem('isFirstTime', 'true');
    } else {
        // If the request has already been made, hide the loading content
        document.querySelector('.loading-content').style.display = 'none';
    }
});

async function FetchData() {
    // Take value of input hidden field
    var fileName = document.getElementById('fileName_input').value;
    $.ajax({
        type: "GET",
        url: "https://localhost:7125/UploadFile_AI_Module/" + fileName,
        dataType: "json",
        success: function (data) {
            const filePath = data.pdfFilePath;

            // Hide loading content
            document.querySelector('.loading-content').style.display = 'none';

            // Display download content
            const downloadContent = document.getElementById('Download-content');
            downloadContent.style.display = 'block';

            var row = '<a id="download_btnn" href="' + filePath + '" download="' + fileName + '">Download File</a>';

            // Append the row string to Download-content
            downloadContent.append(row);
            console.log("File processed successfully. File path:", filePath);
        },

        error: function (error) {
            console.log(error);
        }
    });
}
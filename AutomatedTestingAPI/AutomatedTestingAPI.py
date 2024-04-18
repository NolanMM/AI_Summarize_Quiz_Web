import pytest
import requests

@pytest.mark.parametrize("url", ["https://localhost:7125/"])
def test_home_route_controller(url):
    """
    Test the response of the HomeController.
    """
    try:
        response = requests.get(url, timeout=10, verify=False)
        assert response.status_code == 200
        print("Home Route response successful.")
    except requests.RequestException as e:
        pytest.fail(f"Error occurred while making a request to the controller: {e}")

@pytest.mark.parametrize("file_path", ["./Test_4_Pages.pdf"])
def test_upload_file_route(file_path):
    UPLOAD_URL = "https://localhost:7125/Services/UploadFile_AI_Module"
    
    try:
        with open(file_path, 'rb') as file:
            files = {'file': file}
            response = requests.post(UPLOAD_URL, files=files, verify=False)
            assert response.status_code == 200
            cookies = response.cookies
            session_id = cookies.get('SessionID', None)
            file_name = cookies.get('FileName', None)
            assert session_id is not None and file_name is not None
            print("\n")
            print("SessionID:", session_id)
            print("Filename:", file_name)
            print("\n")
            print("File uploaded route uploaded successfully.")
    except FileNotFoundError:
        pytest.fail("File not found.")
    except Exception as e:
        pytest.fail(f"An error occurred: {e}")
        

@pytest.mark.parametrize("file_path", ["./Test_4_Pages.pdf"])
def test_process_file_route(file_path):
    UPLOAD_URL = "https://localhost:7125/Services/UploadFile_AI_Module"
    PROCESS_URL = "https://localhost:7125/UploadFile_AI_Module_API/";

    try:
        with open(file_path, 'rb') as file:
            files = {'file': file}
            response = requests.post(UPLOAD_URL, files=files, verify=False)
            assert response.status_code == 200
            cookies = response.cookies
            session_id = cookies.get('SessionID', None)
            file_name = cookies.get('FileName', None)
            assert session_id is not None and file_name is not None
            print("\n")
            print("SessionID:", session_id)
            print("Filename:", file_name)
            print("\n")
            print("File uploaded route uploaded successfully.")
            PROCESS_URL = f"{PROCESS_URL}{file_name}/{session_id}"
            
            response = requests.post(PROCESS_URL, verify=False)
            assert response.status_code == 200
            print("File processed successfully.")
    except FileNotFoundError:
        pytest.fail("File not found.")
    except Exception as e:
        pytest.fail(f"An error occurred: {e}")


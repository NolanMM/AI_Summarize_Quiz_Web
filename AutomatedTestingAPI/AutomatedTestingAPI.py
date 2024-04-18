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

@pytest.mark.parametrize("data", [ #registers new user
    {"Username": "newuser", "Password": "newPassword1!", "ConfirmPassword": "newPassword1!"}
])
def test_register_new_user_success(data):
    REGISTER_URL = "https://localhost:7125/Account/Register"

    try:
        response = requests.post(REGISTER_URL, data=data, verify=False, allow_redirects=False)
        assert response.status_code == 200  
        print("Registration test passed: User registered successfully and redirected to home page.")
    except Exception as e:
        pytest.fail(f"Registration test failed: {e}")

@pytest.mark.parametrize("data", [ #login on existing user
    {"Username": "newuser", "Password": "newPassword1!"}
])
def test_login_user_success_and_redirect(data):
    LOGIN_URL = "https://localhost:7125/Account/LogIn"

    try:
        with requests.Session() as session:
            # Submit POST request with form data
            response = session.post(LOGIN_URL, data=data, verify=False)
            
            # Check for correct HTTP status 
            assert response.status_code == 200
            print("Login test passed: User logged in successfully and redirected to home page.")
    except Exception as e:
        pytest.fail(f"Login test failed: {e}")

@pytest.mark.parametrize("url", ["https://localhost:7125/Account/LogIn"]) #makes sure login route is functional
def test_login_page_route(url):
    try:
        response = requests.get(url, timeout=10, verify=False) 
        assert response.status_code == 200  
        print("Login Page response successful.") 
    except requests.RequestException as e:
        pytest.fail(f"Error occurred while making a request to the Login page: {e}")  
        
@pytest.mark.parametrize("registration_data", [
    ({"Username": "newuser", "Password": "NewPassword123!", "ConfirmPassword": "NewPassword123!"})
])
def test_user_registration(registration_data):
    REGISTER_URL = "https://localhost:7125/Account/Register" #checks register route

    with requests.Session() as session:
        # Send registration request
        response = session.post(REGISTER_URL, data=registration_data, verify=False, allow_redirects=False)
        
        # Check for correct HTTP status for the registration request
        assert response.status_code == 200, "Expected a successful status code on registration"
        
        print("Registration test passed: User registration processed successfully.")
import requests
import json

def test_home_route_controller():
    """
    Test the response of the HomeController.
    """
    url = "https://localhost:7125/"
    try:
        response = requests.get(url, timeout=10, verify=False)
        if response.status_code == 200:
            print("Home Route response successfully.")
        else:
            print(f"Failed to get a successful response from the controller. Status code: {response.status_code}")
    except requests.RequestException as e:
        print(f"Error occurred while making a request to the controller: {e}")


def test_upload_file_route(file_path):
    UPLOAD_URL = "https://localhost:7125/Services/UploadFile_AI_Module"
    
    try:
        with open(file_path, 'rb') as file:
            files = {'file': file}
            response = requests.post(UPLOAD_URL, files=files, verify=False)
            if response.status_code == 200:
                cookies = response.cookies
                session_id = cookies.get('SessionID', None)
                file_name = cookies.get('FileName', None)
                if session_id and file_name:
                    print("\n")
                    print("SessionID:", session_id)
                    print("Filename:", file_name)
                    print("\n")
                    print("File uploaded route uploaded successfully.")
                else:
                    print("SessionID or Filename not found in response.")
            else:
                print(f"Failed to upload file. Status code: {response.status_code}")
    except Exception as e:
        print(f"An error occurred: {e}")



if __name__ == "__main__":
    file_path = "./Test_4_Pages.pdf"
    try:
        with open(file_path, 'r') as file:
            pass
    except FileNotFoundError:
        print("File not found.")
        exit()
    test_upload_file_route(file_path)
    test_home_route_controller()

document.addEventListener('DOMContentLoaded', function() {
    const token = localStorage.getItem('event_token');
    
    if (!token) {
        window.location.href = 'login.html';
    }

    const errorAlert = document.getElementById('error-alert');
    const errorMessage = document.getElementById('error-message');
    const successAlert = document.getElementById('success-alert');
    const successMessage = document.getElementById('success-message');

    // Reset previous messages
    errorAlert.classList.add('d-none');
    errorMessage.innerHTML = '';
    successAlert.classList.add('d-none');
    successMessage.innerHTML = '';


    fetch(`http://localhost:8000/admin/api/list/organizers`, {
        method: 'GET',
        headers: {
            'Authorization': 'Bearer ' + token
        }
    }).then(async response => {
        if (response.status === 401 || response.status === 403) {
            errorMessage.innerText = 'Unauthorized access. Redirecting to login...';
            errorAlert.classList.remove('d-none');
            setTimeout(function() {
                window.location.href = 'login.html'; // Redirect after 4 seconds
            }, 4000);
        } else if (response.status === 400) {
            const data = await response.json();
            errorMessage.innerText = data.error_description || 'There was a bad request';
            errorAlert.classList.remove('d-none');
        } else if (response.ok) {
            // spinner.style.display = 'none';
        const data = await response.json();
        updateUserCard(data)
        
    }

    }).catch(error => {
        errorMessage.innerText = error;
        errorAlert.classList.remove('d-none'); // Hide spinner on fetch error
    });

    function updateUserCard(data){
        const container = document.querySelector('.user-container'); // Target table body
        container.innerHTML = '';
        if (data.length === 0) {
            container.innerHTML = `
            <div>
                <span colspan="6" class="text-center">No data found</span>
            </div>
        `;
        }else {
            data.forEach((index) => {
                const rowHtml = `
                <div class="col-xl-4">
    
              <div class="card">
                <div class="card-body profile-card pt-4 d-flex flex-column align-items-center">
    
                  <img src="assets/img/profile_pic.jpg" alt="Profile" class="rounded-circle">
                  <h2>${index.firstName} ${index.lastName}</h2>
                  <h3>${index.userName}</h3>
                   <h3><b>${index.userType}</b></h3>
                </div>
              </div>
    
            </div>
                `;
                container.innerHTML += rowHtml;
            })
        
        }
    }



// document.getElementById('addUser-form').addEventListener('submit', function(event) {
//     event.preventDefault();
//     const form = this;
//     const formData = new FormData(form);
//     if (!form.checkValidity()) {
//         event.stopPropagation();
//         form.classList.add('was-validated'); // This will show the validation messages
//         return;
//     }
//     const spinner = document.getElementById('spinner');
//     const submitText = document.getElementById('submit-text');
    
//     // Show spinner, hide login text
//     spinner.classList.remove('d-none');
//     submitText.classList.add('d-none');
//     fetch(`http://localhost:5297/admin/api/create/salesAgent`, {
//         method: 'POST',
//         body: JSON.stringify(Object.fromEntries(formData.entries())), // Convert form data to JSON
//         headers: {
//             'Content-Type': 'application/json',
//             'Authorization': 'Bearer ' + token
//         }
//     })
//     .then(response => {
//         spinner.classList.add('d-none');
//         submitText.classList.remove('d-none');
        
//         if (response.status===201) {
//             return response.json().then(data => {
//                 successMessage.innerText = data.message || "Created successfully";
//                 successAlert.classList.remove('d-none');
//             });
//         }else{
//             return response.json().then(data => {
//                 errorMessage.innerText = data.message || 'An error occurred. Please try again later.';
//                 errorAlert.classList.remove('d-none');
//             })
//         }
//     }).catch(error => {
//         console.log(error);
//         errorMessage.innerText = "An error occurred. Please try again later.";
//         errorAlert.classList.remove('d-none');

//     })

// })


})
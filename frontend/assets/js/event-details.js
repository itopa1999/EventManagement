document.addEventListener('DOMContentLoaded', function() {
    const token = localStorage.getItem('event_token');
    if (!token) {
        window.location.href = 'login.html';
    }

    function formatCurrency(value) {
        if (value === undefined || value === null) {
            return '₦0.00';
        }
        return '₦' + value.toFixed(2).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ',');
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

    const urlParams = new URLSearchParams(window.location.search);
    const eventId = urlParams.get('id');

    fetch(`http://localhost:8000/admin/api/event/${eventId}/details`, {
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
        } else if (!response.ok) {
            errorMessage.innerText = 'An unexpected error occurred.';
            errorAlert.classList.remove('d-none');
            
        } else {
            // spinner.style.display = 'none';
            const data = await response.json();
            console.log(data);
            updateEventDetails(data.event_details)
            updateEventSessions(data.event_details.sessions)
            updateEventReminders(data.event_details.reminders)

            document.getElementById('tsessions').innerText = data.sessions_count || "N/A";
            document.getElementById('treminders').innerText = data.reminders_count || "N/A";
            document.getElementById('tattendees').innerText = data.attendees_count || "N/A";
            document.getElementById('tinvitations').innerText = data.invitations_count || "N/A";
            document.getElementById('ttickets').innerText = data.tickets_count || "N/A";
            document.getElementById('tpayments').innerText = data.payments_count || "N/A";
            document.getElementById('tFeedbacks').innerText = data.feedbacks_count || "N/A";

        }

    }).catch(error => {
        errorMessage.innerText = 'Error: ' + error;
        errorAlert.classList.remove('d-none');
    });
    



    function updateEventDetails(data){
        

        
        document.getElementById('eventImage').src = data.imagePath;
        document.getElementById('organizerName').innerText = data.organizerFName + " " + data.organizerLName || "N/A";
        document.getElementById('organizerUserName').innerText = data.organizerUName || "N/A";
        document.getElementById('description').innerText = data.description || "N/A";
        document.getElementById('name').innerText = data.name || "N/A";
        document.getElementById('eventType').innerText = data.eventType || "N/A";
        document.getElementById('state').innerText = data.state || "N/A";
        document.getElementById('location').innerText = data.location || "N/A";
        document.getElementById('startDate').innerText = new Date(data.startDate).toLocaleString() || "N/A";
        document.getElementById('endDate').innerText = new Date(data.endDate).toLocaleString() || "N/A";
        document.getElementById('isInvitationOnly').innerText = data.isInvitationOnly ? "Yes" : "No";
        document.getElementById('hasPayment').innerText = data.hasPayment ? "Yes" : "No";
        document.getElementById('price').innerText = formatCurrency(data.price) || "N/A";
        document.getElementById('isBlock').innerText = data.isBlock ? "Yes" : "No";
        document.getElementById('createdAt').innerText = new Date(data.createdAt).toLocaleString() || "N/A";

        document.getElementById('organizerDetails').addEventListener('click', function() {
            window.location.href = `organizer-details.html?id=${data.organizerId}`
        })
    }

    function updateEventSessions(data){
        const sessionBody = document.getElementById('session-container');
        sessionBody.innerHTML = '';
        if (data.length === 0) {
            sessionBody.innerHTML = `
                <tr>
                    <td colspan="6" class="text-center">No data found</td>
                </tr>
            `;
        } else {
            data.forEach((session, index) => {
                const sessionHtml = `
                <div class="col-md-6 mb-4">
                    <div class="card shadow-sm">
                        <div class="card-body">
                        <h5 class="card-title" id="title1">${session.title} (Session #${session.id})</h5>
                        <p class="card-text">
                            <strong>Start Time:</strong> <span "> ${new Date(session.startTime).toLocaleString()}</span><br>
                            <strong>End Time:</strong> <span">${new Date(session.endTime).toLocaleString()}</span><br>
                            <strong>Speaker:</strong> <span id="speaker1">${session.speaker}</span>
                        </p>
                        </div>
                    </div>
                </div>
                    `;
                    sessionBody.innerHTML += sessionHtml;
            })
        }
    }


    function updateEventReminders(data){
        const reminderBody = document.getElementById('reminder-container');
        reminderBody.innerHTML = '';
        if (data.length === 0) {
            reminderBody.innerHTML = `
                <tr>
                    <td colspan="6" class="text-center">No data found</td>
                </tr>
            `;
        } else {
            data.forEach((reminder, index) => {
                const reminderHtml = `
                    <div class="col-md-6 mb-4">
                        <div class="card shadow-sm">
                            <div class="card-body">
                                <h5 class="card-title">Reminder #${reminder.id}</h5>
                                <p class="card-text">
                                    <strong>Reminder Time:</strong> <span>${new Date(reminder.reminderTime).toLocaleString()}</span><br>
                                    <strong>Type:</strong> <span>${reminder.type || "Not Specified"}</span><br>
                                    <strong>Status:</strong> <span>${reminder.hasSent ? "Sent" : "Pending"}</span>
                                </p>
                            </div>
                        </div>
                    </div>
                `;
                reminderBody.innerHTML += reminderHtml;
            })
        }
    }


    document.getElementById('nav-attendees').addEventListener('click', function() { 
        currentPage = 1
        attendeeSearch=''
        document.getElementById('attendee-prevPage').addEventListener('click', function(e) {
            e.preventDefault();
            if (currentPage > 1) {
                currentPage--;
                fetchAttendees();
            }
        });
    
        document.getElementById('attendee-nextPage').addEventListener('click', function(e) {
            e.preventDefault();
            currentPage++;
            fetchAttendees();
        });
    
        document.getElementById('attendee-search').addEventListener('input', function() {
            currentPage = 1;
            attendeeSearch = document.getElementById('attendee-search').value;
            fetchAttendees();
          });
        function fetchAttendees(){
        fetch(`http://localhost:8000/admin/api/event/${eventId}/attendees/details?Search=${attendeeSearch}`, {
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
            } else if (!response.ok) {
                errorMessage.innerText = 'An unexpected error occurred.';
                errorAlert.classList.remove('d-none');
                
            } else {
                // spinner.style.display = 'none';
                const data = await response.json();
                console.log(data)
                const attendeeBody = document.getElementById('attendee-container');
                attendeeBody.innerHTML = '';
                if (data.attendees.length === 0) {
                    attendeeBody.innerHTML = `
                        <tr>
                            <td colspan="6" class="text-center">No data found</td>
                        </tr>
                    `;
                } else {
                    data.attendees.forEach((attendee, index) => {
                        const attendeeHtml = `
                            <div class="col-md-6 mb-4">
                                <div class="card shadow-sm">
                                    <div class="card-body">
                                        <h5 class="card-title">
                                            <i class="bi bi-person-circle"></i> Attendee #${attendee.id}
                                        </h5>
                                        <p class="card-text">
                                            <strong><i class="bi bi-person-fill"></i> Name:</strong> 
                                            <span>${attendee.firstName || "N/A"} ${attendee.lastName || ""}</span><br>
                                            <strong><i class="bi bi-envelope-fill"></i> Email:</strong> 
                                            <span>${attendee.email || "N/A"}</span><br>
                                            <strong><i class="bi bi-telephone-fill"></i> Phone:</strong> 
                                            <span>${attendee.phoneNumber || "N/A"}</span><br>
                                            <strong><i class="bi bi-calendar-check-fill"></i> Registered At:</strong> 
                                            <span>${new Date(attendee.registeredAt).toLocaleString()}</span>
                                        </p>
                                    </div>
                                </div>
                            </div>
                        `;
                        attendeeBody.innerHTML += attendeeHtml; // Append the card HTML to the container
                    });

                    const itemsPerPage = data.pagesize; // Set your items per page here
                    const totalPages = Math.ceil(data.attendees_count / itemsPerPage);
                    document.getElementById('attendee-remaining').innerText = `${currentPage}/${totalPages}`;

                    const nextPageButton = document.getElementById('attendee-nextPage');
                    const prevPageButton = document.getElementById('attendee-prevPage');
                    if (currentPage >= totalPages) {
                        nextPageButton.style.display = 'none'; // Add class to visually disable
                    } else {
                        nextPageButton.style.display = 'block'; // Remove class to enable
                    }
                
                    if (currentPage <= 1) {
                        prevPageButton.style.display = 'none'; // Add class to visually disable
                    } else {
                        prevPageButton.style.display = 'block'; // Remove class to enable
                    }
                    
                }

                

            }
        }).catch(error => {
            errorMessage.innerText = 'Error: ' + error;
            errorAlert.classList.remove('d-none');
        });

    }

    fetchAttendees();
    })

    document.getElementById('nav-invitations').addEventListener('click', function() {
        currentPage = 1
        invitationSearch=''
        document.getElementById('invitation-prevPage').addEventListener('click', function(e) {
            e.preventDefault();
            if (currentPage > 1) {
                currentPage--;
                fetchInvitations();
            }
        });
    
        document.getElementById('invitation-nextPage').addEventListener('click', function(e) {
            e.preventDefault();
            currentPage++;
            fetchInvitations();
        });
    
        document.getElementById('invitation-search').addEventListener('input', function() {
            currentPage = 1;
            invitationSearch = document.getElementById('invitation-search').value;
            fetchInvitations();
          });
        function fetchInvitations(){
        fetch(`http://localhost:8000/admin/api/event/${eventId}/invitation/details?Search=${invitationSearch}`, {
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
            } else if (!response.ok) {
                errorMessage.innerText = 'An unexpected error occurred.';
                errorAlert.classList.remove('d-none');
                
            } else {
                // spinner.style.display = 'none';
                const data = await response.json();
                console.log(data)
                const invitationBody = document.getElementById('invitation-body');
                invitationBody.innerHTML = '';
                if (data.invitations.length === 0) {
                    invitationBody.innerHTML = `
                        <tr>
                            <td colspan="6" class="text-center">No data found</td>
                        </tr>
                    `;
                } else {
                    data.invitations.forEach((invitation, index) => {
                        const rowHtml = `
                            <tr>
                                <td>${invitation.id}</td>
                                <td>${invitation.attendeeEmail}</td>
                                <td>${new Date(invitation.sentAt).toLocaleString()}</td>
                                <td>${invitation.status}</td>
                            </tr>
                        `;
                        invitationBody.innerHTML += rowHtml;
                    
                });

                const itemsPerPage = data.pagesize; // Set your items per page here
                const totalPages = Math.ceil(data.invitations_count / itemsPerPage);
                document.getElementById('invitation-remaining').innerText = `${currentPage}/${totalPages}`;

                const nextPageButton = document.getElementById('invitation-nextPage');
                const prevPageButton = document.getElementById('invitation-prevPage');
                if (currentPage >= totalPages) {
                    nextPageButton.style.display = 'none'; // Add class to visually disable
                } else {
                    nextPageButton.style.display = 'block'; // Remove class to enable
                }
            
                if (currentPage <= 1) {
                    prevPageButton.style.display = 'none'; // Add class to visually disable
                } else {
                    prevPageButton.style.display = 'block'; // Remove class to enable
                }

            }
        
        }
        }).catch(error => {
            errorMessage.innerText = 'Error: ' + error;
            errorAlert.classList.remove('d-none');
        });

    }
    fetchInvitations();
    })


    document.getElementById('nav-tickets').addEventListener('click', function() {
        currentPage = 1
        ticketSearch=''
        document.getElementById('ticket-prevPage').addEventListener('click', function(e) {
            e.preventDefault();
            if (currentPage > 1) {
                currentPage--;
                fetchTickets();
            }
        });
    
        document.getElementById('ticket-nextPage').addEventListener('click', function(e) {
            e.preventDefault();
            currentPage++;
            fetchTickets();
        });
    
        document.getElementById('ticket-search').addEventListener('input', function() {
            currentPage = 1;
            ticketSearch = document.getElementById('ticket-search').value;
            fetchTickets();
          });
        function fetchTickets(){
        fetch(`http://localhost:8000/admin/api/event/${eventId}/tickets/details?Search=${ticketSearch}`, {
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
            } else if (!response.ok) {
                errorMessage.innerText = 'An unexpected error occurred.';
                errorAlert.classList.remove('d-none');
                
            } else {
                // spinner.style.display = 'none';
                const data = await response.json();
                console.log(data)
                const ticketBody = document.getElementById('ticket-container');
                ticketBody.innerHTML = '';
                if (data.tickets.length === 0) {
                    ticketBody.innerHTML = `
                        <tr>
                            <td colspan="6" class="text-center">No data found</td>
                        </tr>
                    `;
                } else {
                    data.tickets.forEach((ticket, index) => {
                        const ticketHtml = `
                            <div class="col-md-6 mb-4">
                                <div class="card shadow-sm">
                                    <div class="card-body">
                                        <h5 class="card-title">Ticket #${ticket.id}</h5>
                                        <p class="card-text">
                                            <strong>Name:</strong> ${ticket.attendeeFirstName} ${ticket.attendeeLastName} <br>
                                            <strong>Email:</strong> ${ticket.attendeeEmail} <br>
                                            <strong>Ticket Type:</strong> ${ticket.ticketType} <br>
                                            <strong>Price:</strong> ${formatCurrency(ticket.price)} <br>
                                            <strong>Checked In:</strong> 
                                            <span class="${ticket.isCheckedIn ? 'checked-in' : 'not-checked-in'}">
                                                ${ticket.isCheckedIn ? 'Yes' : 'No'}
                                            </span> <br>
                                            ${ticket.isCheckedIn ? `<strong>Checked In At:</strong> ${new Date(ticket.checkedInAt).toLocaleString()}` : ''}
                                        </p>
                                    </div>
                                </div>
                            </div>
                        `;
                        ticketBody.innerHTML += ticketHtml;
                    });

                    const itemsPerPage = data.pagesize; // Set your items per page here
                    const totalPages = Math.ceil(data.tickets_count / itemsPerPage);
                    document.getElementById('ticket-remaining').innerText = `${currentPage}/${totalPages}`;

                    const nextPageButton = document.getElementById('ticket-nextPage');
                    const prevPageButton = document.getElementById('ticket-prevPage');
                    if (currentPage >= totalPages) {
                        nextPageButton.style.display = 'none'; // Add class to visually disable
                    } else {
                        nextPageButton.style.display = 'block'; // Remove class to enable
                    }
                
                    if (currentPage <= 1) {
                        prevPageButton.style.display = 'none'; // Add class to visually disable
                    } else {
                        prevPageButton.style.display = 'block'; // Remove class to enable
                    }


            }}
        }).catch(error => {
            errorMessage.innerText = 'Error: ' + error;
            errorAlert.classList.remove('d-none');
        });

    }

    fetchTickets();
    })


    document.getElementById('nav-payments').addEventListener('click', function() {
        currentPage = 1
        paymentSearch=''
        document.getElementById('payment-prevPage').addEventListener('click', function(e) {
            e.preventDefault();
            if (currentPage > 1) {
                currentPage--;
                fetchPayments();
            }
        });
    
        document.getElementById('payment-nextPage').addEventListener('click', function(e) {
            e.preventDefault();
            currentPage++;
            fetchPayments();
        });
    
        document.getElementById('payment-search').addEventListener('input', function() {
            currentPage = 1;
            paymentSearch = document.getElementById('payment-search').value;
            fetchPayments();
          });
        function fetchPayments(){
        fetch(`http://localhost:8000/admin/api/event/${eventId}/payments/details?Search=${paymentSearch}`, {
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
            } else if (!response.ok) {
                errorMessage.innerText = 'An unexpected error occurred.';
                errorAlert.classList.remove('d-none');
                
            } else {
                // spinner.style.display = 'none';
                const data = await response.json();
                console.log(data)
                const paymentBody = document.getElementById('payment-body');
                paymentBody.innerHTML = '';
                if (data.payments.length === 0) {
                    paymentBody.innerHTML = `
                        <tr>
                            <td colspan="6" class="text-center">No data found</td>
                        </tr>
                    `;
                } else {
                    data.payments.forEach((payment, index) => {
                        const rowHtml = `
                            <tr>
                                <td>${payment.id}</td>
                                <td>${payment.payerEmail}</td>
                                <td>${formatCurrency(payment.amount)}</td>
                                <td>${new Date(payment.paymentDate).toLocaleString()}</td>
                                <td>${payment.method}</td>
                                <td>${payment.status}</td>
                                <td>${payment.transactionId}</td>
                            </tr>
                        `;
                        paymentBody.innerHTML += rowHtml;
                    
                })

                const itemsPerPage = data.pagesize; // Set your items per page here
                    const totalPages = Math.ceil(data.payments_count / itemsPerPage);
                    document.getElementById('payment-remaining').innerText = `${currentPage}/${totalPages}`;

                    const nextPageButton = document.getElementById('payment-nextPage');
                    const prevPageButton = document.getElementById('payment-prevPage');
                    if (currentPage >= totalPages) {
                        nextPageButton.style.display = 'none'; // Add class to visually disable
                    } else {
                        nextPageButton.style.display = 'block'; // Remove class to enable
                    }
                
                    if (currentPage <= 1) {
                        prevPageButton.style.display = 'none'; // Add class to visually disable
                    } else {
                        prevPageButton.style.display = 'block'; // Remove class to enable
                    }
            }}
        }).catch(error => {
            errorMessage.innerText = 'Error: ' + error;
            errorAlert.classList.remove('d-none');
        });
    }
    fetchPayments();
    })


    document.getElementById('nav-feedbacks').addEventListener('click', function() {
        currentPage = 1
        feedbackSearch=''
        document.getElementById('feedback-prevPage').addEventListener('click', function(e) {
            e.preventDefault();
            if (currentPage > 1) {
                currentPage--;
                fetchFeedbacks();
            }
        });
    
        document.getElementById('feedback-nextPage').addEventListener('click', function(e) {
            e.preventDefault();
            currentPage++;
            fetchFeedbacks();
        });
    
        document.getElementById('feedback-search').addEventListener('input', function() {
            currentPage = 1;
            feedbackSearch = document.getElementById('feedback-search').value;
            fetchFeedbacks();
          });
        function fetchFeedbacks(){  
        fetch(`http://localhost:8000/admin/api/event/${eventId}/feedbacks/details?Search=${feedbackSearch}`, {
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
            } else if (!response.ok) {
                errorMessage.innerText = 'An unexpected error occurred.';
                errorAlert.classList.remove('d-none');
                
            } else {
                // spinner.style.display = 'none';
                const data = await response.json();
                console.log(data)
                const feedbackBody = document.getElementById('feedback-container');
                feedbackBody.innerHTML = '';
                if (data.feedbacks.length === 0) {
                    feedbackBody.innerHTML = `
                        <tr>
                            <td colspan="6" class="text-center">No data found</td>
                        </tr>
                    `;
                } else {
                    data.feedbacks.forEach((feedback, index) => {
                        const feedbackHtml = `
                            <div class="col-md-6">
                                <div class="card comment-card shadow-sm">
                                    <div class="card-body">
                                        <div class="d-flex align-items-center mb-3">
                                            <i class="bi bi-person-circle" style="font-size: 2rem; margin-right: 10px;"></i>
                                            <div>
                                                <h5 class="card-title">${feedback.attendeeEmail}</h5>
                                                <small class="text-muted">${new Date(feedback.submittedAt).toLocaleString()}</small>
                                            </div>
                                        </div>
                                        <p class="comment-body">${feedback.comments}</p>
                                        <div class="d-flex align-items-center">
                                            <span class="rating">Rating: ${'★'.repeat(feedback.rating)}${'☆'.repeat(5 - feedback.rating)}</span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        `;
                        feedbackBody.innerHTML += feedbackHtml;
                    })

                    const itemsPerPage = data.pagesize; // Set your items per page here
                    const totalPages = Math.ceil(data.feedbacks_count / itemsPerPage);
                    document.getElementById('feedback-remaining').innerText = `${currentPage}/${totalPages}`;

                    const nextPageButton = document.getElementById('feedback-nextPage');
                    const prevPageButton = document.getElementById('feedback-prevPage');
                    if (currentPage >= totalPages) {
                        nextPageButton.style.display = 'none'; // Add class to visually disable
                    } else {
                        nextPageButton.style.display = 'block'; // Remove class to enable
                    }
                
                    if (currentPage <= 1) {
                        prevPageButton.style.display = 'none'; // Add class to visually disable
                    } else {
                        prevPageButton.style.display = 'block'; // Remove class to enable
                    }
            }}
        }).catch(error => {
            errorMessage.innerText = 'Error: ' + error;
            errorAlert.classList.remove('d-none');
        });
    }
    fetchFeedbacks();
    })





});
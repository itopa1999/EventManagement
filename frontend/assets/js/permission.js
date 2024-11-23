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

    organizerCurrentPage = 1
    EventCurrentPage = 1
    AttendeeCurrentPage = 1
    WalletCurrentPage = 1
    AttendeeSearch=''
    walletSearch=''
    organizerSearch=''
    eventSearch=''
    document.getElementById('organizer-prevPage').addEventListener('click', function(e) {
        e.preventDefault();
        if (organizerCurrentPage > 1) {
            organizerCurrentPage--;
            fetchOrganizers();
        }
    });

    document.getElementById('organizer-nextPage').addEventListener('click', function(e) {
        e.preventDefault();
        organizerCurrentPage++;
        fetchOrganizers();
    });

    document.getElementById('organizer-search').addEventListener('input', function() {
        organizerCurrentPage = 1;
        organizerSearch = document.getElementById('organizer-search').value;
        fetchOrganizers();
        });
    function fetchOrganizers(){
    fetch(`http://localhost:8000/admin/api/list/organizers?Search=${organizerSearch}`, {
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
            errorMessage.innerText = "something is wrong.";
            errorAlert.classList.remove('d-none');
        } else {
            const data = await response.json();
            console.log(data)
            const organizerBody = document.getElementById('organizer-body');
            organizerBody.innerHTML = '';
                if (data.organizers.length === 0) {
                    organizerBody.innerHTML = `
                        <tr>
                            <td colspan="6" class="text-center">No data found</td>
                        </tr>
                    `;
                } else {
                    data.organizers.forEach((organizer, index) => {
                        const toggleHtml = organizer.isBlock
                            ? `<label class="switch">
                                <input type="checkbox" class="toggle-input organizer-toggle" data-index="${organizer.id}" checked>
                                <span class="slider round"></span>
                            </label>`
                            : `<label class="switch">
                                <input type="checkbox" class="toggle-input organizer-toggle" data-index="${organizer.id}">
                                <span class="slider round"></span>
                            </label>`;
                        const rowHtml = `
                            <tr>
                                <td>${index + 1}</td>
                                <td>${organizer.firstName}</td>
                                <td>${organizer.lastName}</td>
                                <td><strong><a href="organizer-details.html?id=${organizer.id}">${organizer.userName}</a></strong></td>
                                <td>${organizer.email}</td>
                                <td>${organizer.userType}</td>
                                <td>${organizer.isBlock ? 'Yes' : 'No'}</td>
                                <td>${toggleHtml}</td>
                            </tr>
                        `;
                        organizerBody.innerHTML += rowHtml;
                    
                });

                document.querySelectorAll('.organizer-toggle').forEach(toggle => {
                    toggle.addEventListener('change', function () {
                        const organizerId = this.dataset.index; 
                        const action = this.checked ? "Activate" : "Deactivate";
                        const actionMessage = this.checked ? "Block" : "Unblock";
                        
                        const confirmAction = confirm(`Are you sure you want to ${actionMessage} this organizer?`);
                
                        if (!confirmAction) {
                            this.checked = !this.checked;
                            return;
                        }
                        const url = `http://localhost:8000/admin/api/unblock/${organizerId}/${action}/block/Organizer/access`;
                        fetch(url, {
                            method: 'POST',
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
                                this.checked = !this.checked;
                                return;
                            } else if (!response.ok) {
                                errorMessage.innerText = "something is wrong.";
                                errorAlert.classList.remove('d-none');
                                this.checked = !this.checked;
                                return;
                            } else {
                                const data = await response.json();
                                successAlert.classList.remove('d-none');
                                successMessage.innerText = data.message || 'successful';
                                

                                fetchOrganizers()

                            }
                    }).catch(error => {
                        errorMessage.innerText = error;
                        errorAlert.classList.remove('d-none');
                    });

                       
                    });
                });

                const itemsPerPage = data.pagesize; // Set your items per page here
                const totalPages = Math.ceil(data.organizers_count / itemsPerPage);
                document.getElementById('organizer-remaining').innerText = `${organizerCurrentPage}/${totalPages}`;

                const nextPageButton = document.getElementById('organizer-nextPage');
                const prevPageButton = document.getElementById('organizer-prevPage');
                if (organizerCurrentPage >= totalPages) {
                    nextPageButton.style.display = 'none'; // Add class to visually disable
                } else {
                    nextPageButton.style.display = 'block'; // Remove class to enable
                }
            
                if (organizerCurrentPage <= 1) {
                    prevPageButton.style.display = 'none'; // Add class to visually disable
                } else {
                    prevPageButton.style.display = 'block'; // Remove class to enable
                }

            }


        }
        
        
    })
    .catch(error => {
        errorMessage.innerText = error;
        errorAlert.classList.remove('d-none');
    });

    }
    fetchOrganizers()



    document.getElementById('event-prevPage').addEventListener('click', function(e) {
        e.preventDefault();
        if (EventCurrentPage > 1) {
            EventCurrentPage--;
            fetchEvents();
        }
    });

    document.getElementById('event-nextPage').addEventListener('click', function(e) {
        e.preventDefault();
        EventCurrentPage++;
        fetchEvents();
    });

    document.getElementById('event-search').addEventListener('input', function() {
        EventCurrentPage = 1;
        eventSearch = document.getElementById('event-search').value;
        fetchEvents();
        });
    function fetchEvents(){ 
        fetch(`http://localhost:8000/admin/api/list/events?Name=${eventSearch}`, {
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
                errorMessage.innerText = "something is wrong.";
                errorAlert.classList.remove('d-none');
            } else {
                const data = await response.json();
                console.log(data)
                const eventBody = document.getElementById('event-body');
                eventBody.innerHTML = '';
                    if (data.events.length === 0) {
                        eventBody.innerHTML = `
                            <tr>
                                <td colspan="6" class="text-center">No data found</td>
                            </tr>
                        `;
                    } else {
                        data.events.forEach((event, index) => {
                            const toggleHtml = event.isBlock
                            ? `<label class="switch">
                                <input type="checkbox" class="toggle-input event-toggle" data-index="${event.id}" checked>
                                <span class="slider round"></span>
                            </label>`
                            : `<label class="switch">
                                <input type="checkbox" class="toggle-input event-toggle" data-index="${event.id}">
                                <span class="slider round"></span>
                            </label>`;
                            const rowHtml = `
                                <tr>
                                    <td>${index + 1}</td>
                                    <td><strong><a href="event-details.html?id=${event.id}">${event.name}</a></strong></td>
                                    <td>${event.eventType}</td>
                                    <td>${new Date (event.startDate).toLocaleString()}</td>
                                    <td>${new Date(event.endDate).toLocaleString()}</td>
                                    <td>${event.location}</td>
                                    <td>${event.isBlock ? 'Yes' : 'No'}</td>
                                    <td>${toggleHtml}</td>

                                </tr>
                            `;
                            eventBody.innerHTML += rowHtml;
                        
                    });

                    document.querySelectorAll('.event-toggle').forEach(toggle => {
                        toggle.addEventListener('change', function () {
                            const eventId = this.dataset.index;
                            const action = this.checked ? "Activate" : "Deactivate";
                            const actionMessage = this.checked ? "Block" : "Unblock";
                            
                            const confirmAction = confirm(`Are you sure you want to ${actionMessage} this event?`);
                    
                            if (!confirmAction) {
                                this.checked = !this.checked;
                                return;
                            }
                            const url = `http://localhost:8000/admin/api/unblock/${eventId}/${action}/block/Event/access`;
                                fetch(url, {
                                    method: 'POST',
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
                                        this.checked = !this.checked;
                                        return;
                                    } else if (!response.ok) {
                                        errorMessage.innerText = "something is wrong.";
                                        errorAlert.classList.remove('d-none');
                                        this.checked = !this.checked;
                                        return;
                                    } else {
                                        const data = await response.json();
                                        successMessage.innerText = data.message || 'successful';
                                        successAlert.classList.remove('d-none');

                                        fetchEvents()

                                    }
                            }).catch(error => {
                                errorMessage.innerText = error;
                                errorAlert.classList.remove('d-none');
                            });
                                
                                });
                            });
    
                    const itemsPerPage = data.pagesize; // Set your items per page here
                    const totalPages = Math.ceil(data.events_count / itemsPerPage);
                    document.getElementById('event-remaining').innerText = `${organizerCurrentPage}/${totalPages}`;
    
                    const nextPageButton = document.getElementById('event-nextPage');
                    const prevPageButton = document.getElementById('event-prevPage');
                    if (organizerCurrentPage >= totalPages) {
                        nextPageButton.style.display = 'none'; // Add class to visually disable
                    } else {
                        nextPageButton.style.display = 'block'; // Remove class to enable
                    }
                
                    if (organizerCurrentPage <= 1) {
                        prevPageButton.style.display = 'none'; // Add class to visually disable
                    } else {
                        prevPageButton.style.display = 'block'; // Remove class to enable
                    }
    
                }
    
    
            }
            
            
        })
        .catch(error => {
            errorMessage.innerText = error;
            errorAlert.classList.remove('d-none');
        });
    
        }
        fetchEvents()



        document.getElementById('attendee-prevPage').addEventListener('click', function(e) {
            e.preventDefault();
            if (AttendeeCurrentPage > 1) {
                AttendeeCurrentPage--;
                fetchAttendees();
            }
        });
    
        document.getElementById('attendee-nextPage').addEventListener('click', function(e) {
            e.preventDefault();
            AttendeeCurrentPage++;
            fetchAttendees();
        });
    
        document.getElementById('attendee-search').addEventListener('input', function() {
            AttendeeCurrentPage = 1;
            organizerSearch = document.getElementById('attendee-search').value;
            fetchAttendees();
            });
        function fetchAttendees(){
        fetch(`http://localhost:8000/admin/api/list/attendees?Search=${organizerSearch}`, {
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
                errorMessage.innerText = "something is wrong.";
                errorAlert.classList.remove('d-none');
            } else {
                const data = await response.json();
                console.log(data)
                const attendeeBody = document.getElementById('attendee-body');
                attendeeBody.innerHTML = '';
                    if (data.attendees.length === 0) {
                        attendeeBody.innerHTML = `
                            <tr>
                                <td colspan="6" class="text-center">No data found</td>
                            </tr>
                        `;
                    } else {
                        data.attendees.forEach((attendee, index) => {
                            const toggleHtml = attendee.isBlock
                            ? `<label class="switch">
                                <input type="checkbox" class="toggle-input attendee-toggle" data-index="${attendee.email}" checked>
                                <span class="slider round"></span>
                            </label>`
                            : `<label class="switch">
                                <input type="checkbox" class="toggle-input attendee-toggle" data-index="${attendee.email}">
                                <span class="slider round"></span>
                            </label>`;
                            const rowHtml = `
                                <tr>
                                    <td>${index + 1}</td>
                                    <td>${attendee.firstName}</td>
                                    <td>${attendee.lastName}</td>
                                    <td><strong><a href="attendee-details.html?id=${attendee.id}">${attendee.email}</a></strong></td>
                                    <td>${attendee.isBlock ? 'Yes' : 'No'}</td>
                                    <td>${toggleHtml}</td>
                                </tr>
                            `;
                            attendeeBody.innerHTML += rowHtml;
                        
                    });

                    document.querySelectorAll('.attendee-toggle').forEach(toggle => {
                        toggle.addEventListener('change', function() {
                            const attendeeEmail = this.dataset.index; 
                            const action = this.checked ? "Activate" : "Deactivate";
                            const actionMessage = this.checked ? "Block" : "Unblock";
                    
                            const confirmAction = confirm(`Are you sure you want to ${actionMessage} this attendee email?`);

                            if (!confirmAction) {
                                this.checked = !this.checked;
                                return;
                            }
                            const url = `http://localhost:8000/admin/api/unblock/${attendeeEmail}/${action}/block/email/access`;
                                fetch(url, {
                                    method: 'POST',
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
                                        this.checked = !this.checked;
                                        return;
                                    } else if (!response.ok) {
                                        errorMessage.innerText = response.json();
                                        errorAlert.classList.remove('d-none');
                                        this.checked = !this.checked;
                                        return;
                                    } else {
                                        const data = await response.json();
                                        successMessage.innerText = data.message || 'successful';
                                        successAlert.classList.remove('d-none');

                                        fetchAttendees()

                                    }
                            }).catch(error => {
                                errorMessage.innerText = error;
                                errorAlert.classList.remove('d-none');
                            });
                                
                                });
                            });


                    const itemsPerPage = data.pagesize; // Set your items per page here
                    const totalPages = Math.ceil(data.attendees_count / itemsPerPage);
                    document.getElementById('attendee-remaining').innerText = `${AttendeeCurrentPage}/${totalPages}`;
    
                    const nextPageButton = document.getElementById('attendee-nextPage');
                    const prevPageButton = document.getElementById('attendee-prevPage');
                    if (AttendeeCurrentPage >= totalPages) {
                        nextPageButton.style.display = 'none'; // Add class to visually disable
                    } else {
                        nextPageButton.style.display = 'block'; // Remove class to enable
                    }
                
                    if (AttendeeCurrentPage <= 1) {
                        prevPageButton.style.display = 'none'; // Add class to visually disable
                    } else {
                        prevPageButton.style.display = 'block'; // Remove class to enable
                    }
    
                }
    
    
            }
            
            
        })
        .catch(error => {
            errorMessage.innerText = error;
            errorAlert.classList.remove('d-none');
        });
    
        }
        fetchAttendees()





        document.getElementById('wallet-prevPage').addEventListener('click', function(e) {
            e.preventDefault();
            if (WalletCurrentPage > 1) {
                WalletCurrentPage--;
                fetchWallet();
            }
        });
    
        document.getElementById('wallet-nextPage').addEventListener('click', function(e) {
            e.preventDefault();
            WalletCurrentPage++;
            fetchWallet();
        });
    
        document.getElementById('wallet-search').addEventListener('input', function() {
            WalletCurrentPage = 1;
            organizerSearch = document.getElementById('wallet-search').value;
            fetchWallet();
            });
        function fetchWallet(){
        fetch(`http://localhost:8000/admin/api/list/organizers/wallet?Search=${organizerSearch}`, {
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
                errorMessage.innerText = "something is wrong.";
                errorAlert.classList.remove('d-none');
            } else {
                const data = await response.json();
                console.log(data)
                const walletBody = document.getElementById('wallet-body');
                walletBody.innerHTML = '';
                    if (data.wallets.length === 0) {
                        walletBody.innerHTML = `
                            <tr>
                                <td colspan="6" class="text-center">No data found</td>
                            </tr>
                        `;
                    } else {
                        data.wallets.forEach((wallet, index) => {
                            const toggleHtml = wallet.isBlock
                            ? `<label class="switch">
                                <input type="checkbox" class="toggle-input wallet-toggle" data-index="${wallet.userId}" checked>
                                <span class="slider round"></span>
                            </label>`
                            : `<label class="switch">
                                <input type="checkbox" class="toggle-input wallet-toggle" data-index="${wallet.userId}">
                                <span class="slider round"></span>
                            </label>`;
                            const rowHtml = `
                                <tr>
                                    <td>${index + 1}</td>
                                    <td>${wallet.firstName}</td>
                                    <td>${wallet.lastName}</td>
                                    <td>${wallet.email}</td>
                                    <td><strong><a href="organizer-details.html?id=${wallet.userId}">${wallet.username}</a></strong></td>
                                    <td>${wallet.balance}</td>
                                    <td>${wallet.isBlock ? 'Yes' : 'No'}</td>
                                    <td>${toggleHtml}</td>
                                    
                                </tr>
                            `;

                            walletBody.innerHTML += rowHtml;
                    });

                    document.querySelectorAll('.wallet-toggle').forEach(toggle => {
                        toggle.addEventListener('change', function() {
                            const walletId = this.dataset.index; 
                            const action = this.checked ? "Activate" : "Deactivate";
                            const actionMessage = this.checked ? "Block" : "Unblock";
                    
                            const confirmAction = confirm(`Are you sure you want to ${actionMessage} this wallet?`);

                            if (!confirmAction) {
                                this.checked = !this.checked;
                                return;
                            }
                            const url = `http://localhost:8000/admin/api/unblock/${walletId}/${action}/block/wallet/access`;
                                fetch(url, {
                                    method: 'POST',
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
                                        this.checked = !this.checked;
                                        return;
                                    } else if (!response.ok) {
                                        errorMessage.innerText = response.json();
                                        errorAlert.classList.remove('d-none');
                                        this.checked = !this.checked;
                                        return;
                                    } else {
                                        const data = await response.json();
                                        successMessage.innerText = data.message || 'successful';
                                        successAlert.classList.remove('d-none');

                                        fetchWallet()

                                    }
                            }).catch(error => {
                                errorMessage.innerText = error;
                                errorAlert.classList.remove('d-none');
                            });
                                
                                });
                            });
    
                    const itemsPerPage = data.pagesize; // Set your items per page here
                    const totalPages = Math.ceil(data.wallets_count / itemsPerPage);
                    document.getElementById('wallet-remaining').innerText = `${WalletCurrentPage}/${totalPages}`;
    
                    const nextPageButton = document.getElementById('wallet-nextPage');
                    const prevPageButton = document.getElementById('wallet-prevPage');
                    if (WalletCurrentPage >= totalPages) {
                        nextPageButton.style.display = 'none'; // Add class to visually disable
                    } else {
                        nextPageButton.style.display = 'block'; // Remove class to enable
                    }
                
                    if (WalletCurrentPage <= 1) {
                        prevPageButton.style.display = 'none'; // Add class to visually disable
                    } else {
                        prevPageButton.style.display = 'block'; // Remove class to enable
                    }
    
                }
    
    
            }
            
            
        })
        .catch(error => {
            errorMessage.innerText = error;
            errorAlert.classList.remove('d-none');
        });
    
        }
        fetchWallet()
    


})

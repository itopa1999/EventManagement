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

    function formatCurrency(value) {
        if (value === undefined || value === null) {
            return '₦0.00'; // or any default value you'd prefer
        }
        return '₦' + value.toFixed(2).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ',');
    }

    CurrentPage = 1
    transactionSearch=''

    document.getElementById('transaction-prevPage').addEventListener('click', function(e) {
        e.preventDefault();
        if (CurrentPage > 1) {
            CurrentPage--;
            fetchTransactions();
        }
    });

    document.getElementById('transaction-nextPage').addEventListener('click', function(e) {
        e.preventDefault();
        CurrentPage++;
        fetchTransactions();
    });

    document.getElementById('transaction-search').addEventListener('input', function() {
        CurrentPage = 1;
        transactionSearch = document.getElementById('transaction-search').value;
        fetchTransactions();
        });
    function fetchTransactions(){
    fetch(`http://localhost:8000/admin/api/list/transactions?Search=${transactionSearch}`, {
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
            const organizerBody = document.getElementById('transaction-body');
            organizerBody.innerHTML = '';
                if (data.transactions.length === 0) {
                    organizerBody.innerHTML = `
                        <tr>
                            <td colspan="6" class="text-center">No data found</td>
                        </tr>
                    `;
                } else {
                    data.transactions.forEach((transaction, index) => {
                        const rowHtml = `
                            <tr class="details-btn" 
                            id="details-${transaction.id}"
                            id="details-${transaction.id}"
                            id="details-${transaction.id}"
                            id="details-${transaction.id}"
                            id="details-${transaction.id}"
                            id="details-${transaction.id}"
                            id="details-${transaction.id}"
                            id="details-${transaction.id}">
                            id="details-${transaction.id}"
                            id="details-${transaction.id}"
                            id="details-${transaction.id}"
                                <td>${transaction.id}</td>
                                <td>${transaction.username}</td>
                                <td>${formatCurrency(transaction.amount)}</td>
                                <td>${new Date (transaction.date).toLocaleString()}</td>
                            </tr>
                        `;
                        organizerBody.innerHTML += rowHtml;
                    
                });

                document.querySelectorAll('.details-btn').forEach(button => {
                    button.addEventListener('click', function () {
                        const transactionId = this.id.replace('details-', '');
                        alert(transactionId);
                    });
                });
                
                const itemsPerPage = data.pagesize; // Set your items per page here
                const totalPages = Math.ceil(data.transactions_count / itemsPerPage);
                document.getElementById('transaction-remaining').innerText = `${CurrentPage}/${totalPages}`;

                const nextPageButton = document.getElementById('transaction-nextPage');
                const prevPageButton = document.getElementById('transaction-prevPage');
                if (CurrentPage >= totalPages) {
                    nextPageButton.style.display = 'none'; // Add class to visually disable
                } else {
                    nextPageButton.style.display = 'block'; // Remove class to enable
                }
            
                if (CurrentPage <= 1) {
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
    fetchTransactions()











})
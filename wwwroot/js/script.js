const apiUrl = "/book"; // כתובת ה-API של השרת

document.addEventListener("DOMContentLoaded", loadBooks);

function loadBooks() {
    fetch(apiUrl)
        .then(response => response.json())
        .then(books => {
            const tableBody = document.getElementById("bookList");
            tableBody.innerHTML = "";
            books.forEach(book => {
                let row = `<tr>
                    <td>${book.id}</td>
                    <td contenteditable="true" onBlur="updateBook(${book.id}, this, 'name')">${book.name}</td>
                    <td contenteditable="true" onBlur="updateBook(${book.id}, this, 'author')">${book.author}</td>
                    <td>
                        <button class="delete" onclick="deleteBook(${book.id})">Delete</button>
                    </td>
                </tr>`;
                tableBody.innerHTML += row;
            });
        });
}

document.getElementById("addBookForm").addEventListener("submit", function (event) {
    event.preventDefault();
    const name = document.getElementById("bookName").value;
    const author = document.getElementById("bookAuthor").value;

    fetch(apiUrl, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ name, author })
    }).then(response => response.json())
      .then(() => {
          document.getElementById("bookName").value = "";
          document.getElementById("bookAuthor").value = "";
          loadBooks();
      });
});

function deleteBook(id) {
    fetch(`${apiUrl}/${id}`, { method: "DELETE" })
        .then(response => {
            if (response.ok) loadBooks();
        });
}

function updateBook(id, element, field) {
    const newValue = element.innerText;
    fetch(`${apiUrl}/${id}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ id, [field]: newValue })
    });
}

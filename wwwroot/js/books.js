export  const loadBooks=async(containerId = "bookList") =>{
    const token = localStorage.getItem("token");
    const res = await fetch("http://localhost:5017/book", {
        headers: { "Authorization": `Bearer ${token}` }
    });
    if (!res.ok) return;

    const books = await res.json();
    const container = document.getElementById(containerId);
    if (!container) return;
    container.innerHTML = "";

    books.forEach(book => {
        const div = document.createElement("div");
        div.innerHTML = `
            <b>${book.name}</b> סופר: ${book.author}
            <button onclick="editBook(${book.id}, '${book.name}', '${book.author}')">✏️</button>
            <button onclick="deleteBook(${book.id})">🗑️</button>
        `;
        container.appendChild(div);
    });
}

export  const deleteBook =async (id)=> {
    const token = localStorage.getItem("token");
    const res = await fetch(`http://localhost:5017/book/${id}`, {
        method: "DELETE",
        headers: { "Authorization": `Bearer ${token}` }
    });
    if (res.ok) {
        loadBooks();
    } else {
        alert("שגיאה במחיקה");
    }
}

export const editBook = (id, name, author)=> {
    document.getElementById("editBookId").value = id;
    document.getElementById("editBookName").value = name;
    document.getElementById("editBookAuthor").value = author;
    document.getElementById("editBookFormContainer").style.display = "block";
}

console.log("נרשמה הפונקציה editBook ל-window");

window.editBook = editBook;
window.deleteBook = deleteBook;


import { loadBooks, deleteBook, editBook } from './books.js';

window.onload = async () => {
    const token = localStorage.getItem("token");
    if (!token) return window.location.href = "index.html";

    const userRes = await fetch("http://localhost:5017/user/me", {
        headers: { "Authorization": `Bearer ${token}` }
    });

    if (!userRes.ok) {
        localStorage.clear();
        return window.location.href = "index.html";
    }

    const user = await userRes.json();
    document.getElementById("profile").innerText = `שלום ${user.name}, תפקיד: ${user.role}`;
    
    if (user.role === "Admin") {
        const link = document.createElement("a");
        link.href = "users.html";
        link.innerText = "ניהול משתמשים";
        link.style.display = "inline-block";
        link.style.marginRight = "20px";
        document.body.insertBefore(link, document.getElementById("bookList"));
    }

    await loadBooks("bookList");

    // טיפול בטופס הוספת ספר
    document.getElementById("addBookForm").addEventListener("submit", async (e) => {
        e.preventDefault();
        const name = document.getElementById("bookName").value;
        const author = document.getElementById("bookAuthor").value;

        const res = await fetch("http://localhost:5017/book", {
            method: "POST",
            headers: {
                "Authorization": `Bearer ${token}`,
                "Content-Type": "application/json"
            },
            body: JSON.stringify({ name, author })
        });

        if (res.ok) {
            document.getElementById("addBookForm").reset();
            document.getElementById("addBookForm").style.display = "none";
            document.getElementById("toggleAddBookBtn").style.display = "inline-block";
            await loadBooks("bookList");
            showMessage("✅ הספר נוסף בהצלחה");
        } else {
            showMessage("❌ שגיאה בהוספת ספר");
        }
    });

    // טיפול בטופס עריכת ספר
    document.getElementById("editBookForm").addEventListener("submit", async (e) => {
        e.preventDefault();
        const id = +document.getElementById("editBookId").value;
        const name = document.getElementById("editBookName").value;
        const author = document.getElementById("editBookAuthor").value;

        const res = await fetch(`http://localhost:5017/book/${id}`, {
            method: "PUT",
            headers: {
                "Authorization": `Bearer ${token}`,
                "Content-Type": "application/json"
            },
            body: JSON.stringify({ name, author })
        });

        if (res.ok) {
            showMessage("✅ הספר עודכן בהצלחה");
            document.getElementById("editBookFormContainer").style.display = "none";
            await loadBooks("bookList");
        } else {
            showMessage("❌ שגיאה בעדכון הספר");
        }
    });

    // כפתור פתיחת טופס הוספת ספר
    document.getElementById("toggleAddBookBtn").addEventListener("click", () => {
        const form = document.getElementById("addBookForm");
        form.style.display = "block";
        document.getElementById("toggleAddBookBtn").style.display = "none";
    });
};

// הודעה זמנית שנעלמת אחרי כמה שניות
function showMessage(msg) {
    let m = document.getElementById("messageBox");
    if (!m) {
        m = document.createElement("div");
        m.id = "messageBox";
        m.style.color = "green";
        m.style.marginTop = "10px";
        document.body.appendChild(m);
    }
    m.innerText = msg;
    setTimeout(() => m.innerText = "", 3000);
}

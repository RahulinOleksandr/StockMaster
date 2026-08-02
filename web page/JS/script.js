const BASE_URL = "http://localhost:5239/api";

// --- Tab switching ---
document.querySelectorAll(".tab-btn").forEach(btn => {
    btn.addEventListener("click", () => {
        document.querySelectorAll(".tab-btn").forEach(b => b.classList.remove("active"));
        document.querySelectorAll(".panel").forEach(p => p.classList.remove("active"));
        btn.classList.add("active");
        document.getElementById(btn.dataset.tab + "-panel").classList.add("active");
    });
});

function showError(message) {
    document.getElementById("error").textContent = message;
}

// Generic helper for requests (avoids duplicating try/catch everywhere)
async function apiCall(url, method = "GET", body = null) {
    const options = { method };
    if (body) {
        options.headers = { "Content-Type": "application/json" };
        options.body = JSON.stringify(body);
    }
    const res = await fetch(url, options);

    if (!res.ok) {
        // Try to read validation details from the response body
        let message = `Server error: ${res.status}`;
        try {
            const errorData = await res.json();
            if (errorData.errors) {
                // ASP.NET Core ModelState format: { errors: { fieldName: ["msg1", "msg2"] } }
                const messages = Object.values(errorData.errors).flat();
                message = messages.join("; ");
            } else if (errorData.title) {
                message = errorData.title;
            }
        } catch {
            // Response wasn't JSON (e.g. plain 404) - keep the generic message
        }
        throw new Error(message);
    }

    if (res.status === 204) return null; // No Content (typical for PUT/DELETE)
    return res.json();
}

// ===================== PRODUCTS =====================

let allProducts = []; // cached so we don't refetch for the stock form dropdown
let allCategories = []; // cached for the product form dropdown

async function loadProducts() {
    showError("");
    const tbody = document.getElementById("productsBody");
    tbody.innerHTML = "";
    try {
        allProducts = await apiCall(`${BASE_URL}/products`);
        allProducts.forEach(p => {
            const row = document.createElement("tr");
            row.innerHTML = `
                        <td>${p.productId}</td><td>${p.productName}</td><td>${p.categoryName}</td><td>${p.price} ₴</td>
                        <td>
                            <button class="edit" onclick='openProductForm(${JSON.stringify(p)})'>✏️</button>
                            <button class="danger" onclick="deleteProduct(${p.productId})">🗑️</button>
                        </td>`;
            tbody.appendChild(row);
        });
        fillProductSelect();
    } catch (err) {
        showError("Помилка завантаження товарів: " + err.message);
    }
}

function openProductForm(product = null) {
    document.getElementById("productForm").classList.add("active");
    document.getElementById("productId").value = product ? product.productId : "";
    document.getElementById("productName").value = product ? product.productName : "";
    document.getElementById("productCategoryId").value = product ? product.categoryId : "";
    document.getElementById("productPrice").value = product ? product.price : "";
}

function closeProductForm() {
    document.getElementById("productForm").classList.remove("active");
}

async function saveProduct() {
    const id = document.getElementById("productId").value;
    const data = {
        productName: document.getElementById("productName").value,
        CategoryId: parseInt(document.getElementById("productCategoryId").value) || 0,
        price: parseFloat(document.getElementById("productPrice").value) || 0
    };
    try {
        if (id) {
            await apiCall(`${BASE_URL}/products/${id}`, "PUT", data); // update
        } else {
            await apiCall(`${BASE_URL}/products`, "POST", data); // create
        }
        closeProductForm();
        loadProducts();
        loadWarehouses();
        loadCategories();
        loadStock();
    } catch (err) {
        showError("Помилка збереження товару: " + err.message);
    }
}

async function deleteProduct(id) {
    if (!confirm("Видалити цей товар?")) return;
    try {
        await apiCall(`${BASE_URL}/products/${id}`, "DELETE");
        loadProducts();
    } catch (err) {
        showError("Помилка видалення товару: " + err.message);
    }
}

// ===================== WAREHOUSES =====================

let allWarehouses = [];

async function loadWarehouses() {
    showError("");
    const tbody = document.getElementById("warehousesBody");
    tbody.innerHTML = "";
    try {
        allWarehouses = await apiCall(`${BASE_URL}/warehouses`);
        allWarehouses.forEach(w => {
            const row = document.createElement("tr");
            row.innerHTML = `
                        <td>${w.warehouseId}</td><td>${w.warehouseName}</td><td>${w.address}</td>
                        <td>
                            <button class="edit" onclick='openWarehouseForm(${JSON.stringify(w)})'>✏️</button>
                            <button class="danger" onclick="deleteWarehouse(${w.warehouseId})">🗑️</button>
                        </td>`;
            tbody.appendChild(row);
        });
        fillWarehouseSelect();
    } catch (err) {
        showError("Помилка завантаження складів: " + err.message);
    }
}

function openWarehouseForm(warehouse = null) {
    document.getElementById("warehouseForm").classList.add("active");
    document.getElementById("warehouseId").value = warehouse ? warehouse.warehouseId : "";
    document.getElementById("warehouseName").value = warehouse ? warehouse.warehouseName : "";
    document.getElementById("warehouseAddress").value = warehouse ? warehouse.address : "";
}

function closeWarehouseForm() {
    document.getElementById("warehouseForm").classList.remove("active");
}

async function saveWarehouse() {
    const id = document.getElementById("warehouseId").value;
    const data = {
        warehouseName: document.getElementById("warehouseName").value,
        address: document.getElementById("warehouseAddress").value
    };
    try {
        if (id) {
            await apiCall(`${BASE_URL}/warehouses/${id}`, "PUT", data);
        } else {
            await apiCall(`${BASE_URL}/warehouses`, "POST", data);
        }
        closeWarehouseForm();
        loadProducts();
        loadWarehouses();
        loadCategories();
        loadStock();
    } catch (err) {
        showError("Помилка збереження складу: " + err.message);
    }
}

async function deleteWarehouse(id) {
    if (!confirm("Видалити цей склад?")) return;
    try {
        await apiCall(`${BASE_URL}/warehouses/${id}`, "DELETE");
        loadWarehouses();
    } catch (err) {
        showError("Помилка видалення складу: " + err.message);
    }
}

// ===================== CATEGORIES =====================

async function loadCategories() {
    showError("");
    const tbody = document.getElementById("categoriesBody");
    tbody.innerHTML = "";
    try {
        allCategories = await apiCall(`${BASE_URL}/categories`);
        allCategories.forEach(c => {
            const row = document.createElement("tr");
            row.innerHTML = `
                        <td>${c.categoryId}</td><td>${c.categoryName}</td>
                        <td>
                            <button class="edit" onclick='openCategoryForm(${JSON.stringify(c)})'>✏️</button>
                            <button class="danger" onclick="deleteCategory(${c.categoryId})">🗑️</button>
                        </td>`;
            tbody.appendChild(row);
        });
        fillCategorySelect(); // update the products form dropdown too
    } catch (err) {
        showError("Помилка завантаження категорій: " + err.message);
    }
}

function openCategoryForm(category = null) {
    document.getElementById("categoryForm").classList.add("active");
    document.getElementById("categoryId").value = category ? category.categoryId : "";
    document.getElementById("categoryName").value = category ? category.categoryName : "";
}

function closeCategoryForm() {
    document.getElementById("categoryForm").classList.remove("active");
}

async function saveCategory() {
    const id = document.getElementById("categoryId").value;
    const data = { categoryName: document.getElementById("categoryName").value };
    try {
        if (id) {
            await apiCall(`${BASE_URL}/categories/${id}`, "PUT", data);
        } else {
            await apiCall(`${BASE_URL}/categories`, "POST", data);
        }
        closeCategoryForm();
        loadProducts();
        loadWarehouses();
        loadCategories();
        loadStock();
    } catch (err) {
        showError("Помилка збереження категорії: " + err.message);
    }
}

async function deleteCategory(id) {
    if (!confirm("Видалити цю категорію?")) return;
    try {
        await apiCall(`${BASE_URL}/categories/${id}`, "DELETE");
        loadCategories();
    } catch (err) {
        showError("Помилка видалення категорії: " + err.message);
    }
}

function fillCategorySelect() {
    const select = document.getElementById("productCategoryId");
    select.innerHTML = '<option value="">Категорія...</option>';
    allCategories.forEach(c => {
        select.innerHTML += `<option value="${c.categoryId}">${c.categoryName}</option>`;
    });
}

// ===================== STOCK =====================

async function loadStock() {
    showError("");
    const tbody = document.getElementById("stockBody");
    tbody.innerHTML = "";
    try {
        const stocks = await apiCall(`${BASE_URL}/stock`);
        stocks.forEach(s => {
            const row = document.createElement("tr");
            row.innerHTML = `
                        <td>${s.productName}</td><td>${s.warehouseName}</td>
                        <td><span class="badge">${s.quantity} шт</span></td>
                        <td><button class="danger" onclick="deleteStock(${s.stockId})">🗑️</button></td>`;
            tbody.appendChild(row);
        });
    } catch (err) {
        showError("Помилка завантаження залишків: " + err.message);
    }
}

// Populate product/warehouse dropdowns for the stock form
function fillProductSelect() {
    const select = document.getElementById("stockProductId");
    select.innerHTML = '<option value="">Товар...</option>';
    allProducts.forEach(p => {
        select.innerHTML += `<option value="${p.productId}">${p.productName}</option>`;
    });
}

function fillWarehouseSelect() {
    const select = document.getElementById("stockWarehouseId");
    select.innerHTML = '<option value="">Склад...</option>';
    allWarehouses.forEach(w => {
        select.innerHTML += `<option value="${w.warehouseId}">${w.warehouseName}</option>`;
    });
}

function openStockForm() {
    document.getElementById("stockForm").classList.add("active");
}

function closeStockForm() {
    document.getElementById("stockForm").classList.remove("active");
}

async function saveStock() {
    const data = {
        productId: parseInt(document.getElementById("stockProductId").value),
        warehouseId: parseInt(document.getElementById("stockWarehouseId").value),
        quantity: parseInt(document.getElementById("stockQuantity").value) || 0
    };
    if (!data.productId || !data.warehouseId) {
        showError("Обери товар і склад перед збереженням");
        return;
    }
    try {
        await apiCall(`${BASE_URL}/stock`, "POST", data);
        closeStockForm();
        loadProducts();
        loadWarehouses();
        loadCategories();
        loadStock();
    } catch (err) {
        showError("Помилка збереження залишку: " + err.message);
    }
}

async function deleteStock(id) {
    if (!confirm("Видалити цей запис про залишок?")) return;
    try {
        await apiCall(`${BASE_URL}/stock/${id}`, "DELETE");
        loadStock();
    } catch (err) {
        showError("Помилка видалення залишку: " + err.message);
    }
}

// Load everything right when the page opens
loadProducts();
loadWarehouses();
loadCategories();
loadStock();
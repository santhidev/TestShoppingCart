// pages/index.js
import { useEffect, useState } from 'react';

export default function Home() {
  const [products, setProducts] = useState([]);

  useEffect(() => {
    fetch('http://localhost:5000/api/products')
      .then((response) => response.json())
      .then((data) => setProducts(data));
  }, []);

  return (
    <div className="container mx-auto p-4">
      <h1 className="text-3xl font-bold text-center mb-8">Products</h1>
      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4">
        {products.map((product) => (
          <div key={product.productId} className="border p-4 rounded shadow hover:shadow-lg transition-shadow duration-300">
            <h2 className="text-xl font-semibold">{product.name}</h2>
            <p className="text-gray-600">${product.price}</p>
            <button
              className="mt-4 bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600 transition-colors duration-300"
              onClick={() => addToCart(product.productId)}
            >
              Add to Cart
            </button>
          </div>
        ))}
      </div>
    </div>
  );

  function addToCart(productId) {
    fetch(`http://localhost:5000/api/cart/add?productId=${productId}&quantity=1`, { method: 'POST' })
      .then((response) => {
        if (response.ok) {
          alert('Product added to cart');
        } else {
          alert('Failed to add product to cart');
        }
      });
  }
}

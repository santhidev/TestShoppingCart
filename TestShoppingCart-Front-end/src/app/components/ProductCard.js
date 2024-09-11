"use client";

import { useState } from 'react';

export default function ProductCard({ product }) {
  const [isAdded, setIsAdded] = useState(false);

  const addToCart = async () => {
    try {
      const response = await fetch('https://localhost:7042/api/cart/add', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          productId: product.productId,
          name: product.name,
          price: product.price,
          quantity: 1
        }),
        credentials: 'include'
      });

      if (response.ok) {
        setIsAdded(true);
        setTimeout(() => setIsAdded(false), 2000); // ซ่อนข้อความแจ้งเตือนหลัง 2 วินาที
      } else {
        const errorText = await response.text();
        alert(`Failed to add product to cart: ${errorText}`);
      }
    } catch (error) {
      console.error('Failed to add item:', error);
    }
  };

  return (
    <div className="border p-6 rounded-lg shadow hover:shadow-lg transition-shadow duration-300 bg-white">
      <h2 className="text-lg font-semibold mb-2">{product.name}</h2>
      <p className="text-gray-600 mb-2">Price: ${product.price}</p>
      <p className="text-gray-500 mb-4">Stock: {product.stockQuantity}</p>
      <button
        className={`w-full py-2 rounded ${isAdded ? 'bg-green-500' : 'bg-blue-500'} text-white hover:bg-blue-600 transition-colors duration-300`}
        onClick={addToCart}
        disabled={isAdded}
      >
        {isAdded ? 'Added to Cart' : 'Add to Cart'}
      </button>
    </div>
  );
}

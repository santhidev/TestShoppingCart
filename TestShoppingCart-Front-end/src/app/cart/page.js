"use client";

import { useEffect, useState } from 'react';
import Link from 'next/link';

export default function Cart() {
  const [cartItems, setCartItems] = useState([]);
  const [totalAmount, setTotalAmount] = useState(0);
  const [showPayment, setShowPayment] = useState(false);
  const [paymentSuccess, setPaymentSuccess] = useState(false);
  const [errorMessage, setErrorMessage] = useState(''); // สถานะสำหรับข้อความแจ้งเตือน

  useEffect(() => {
    fetchCartItems();
  }, []);

  useEffect(() => {
    calculateTotalAmount();
  }, [cartItems]);

  const fetchCartItems = async () => {
    try {
      const response = await fetch('https://localhost:7042/api/cart', {
        credentials: 'include',
      });
      if (!response.ok) throw new Error('Failed to fetch cart items');
      const data = await response.json();
      setCartItems(data);
    } catch (error) {
      console.error('Error fetching cart items:', error);
    }
  };

  const calculateTotalAmount = () => {
    const total = cartItems.reduce((sum, item) => sum + item.price * item.quantity, 0);
    setTotalAmount(total);
  };

  const handleIncreaseQuantity = async (productId, name) => {
    const item = cartItems.find((item) => item.productId === productId);
    if (item.quantity >= item.stockQuantity) {
      setErrorMessage('Cannot add more than available stock.');
      alert('Cannot add more than available stock.'); // แสดง Alert แจ้งเตือน
      setTimeout(() => setErrorMessage(''), 3000); // ซ่อนข้อความแจ้งเตือนหลังจาก 3 วินาที
      return;
    }

    try {
      await fetch('https://localhost:7042/api/cart/increase', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ productId, name }),
        credentials: 'include',
      });
      fetchCartItems(); // Update cart items after increasing quantity
    } catch (error) {
      console.error('Failed to increase item quantity:', error);
    }
  };

  const handleDecreaseQuantity = async (productId, name) => {
    try {
      await fetch('https://localhost:7042/api/cart/decrease', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ productId, name }),
        credentials: 'include',
      });
      fetchCartItems(); // Update cart items after decreasing quantity
    } catch (error) {
      console.error('Failed to decrease item quantity:', error);
    }
  };

  const removeFromCart = async (productId) => {
    try {
      const response = await fetch('https://localhost:7042/api/cart/remove', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ productId }),
        credentials: 'include',
      });

      if (!response.ok) {
        const errorText = await response.text();
        alert(`Failed to remove product from cart: ${errorText}`);
      } else {
        fetchCartItems(); // Update cart items after removal
      }
    } catch (error) {
      console.error('Failed to remove item from cart:', error);
    }
  };

  const handleClearCart = async () => {
    try {
      await fetch('https://localhost:7042/api/cart/clear', {
        method: 'POST',
        credentials: 'include',
      });
      fetchCartItems(); // Update cart after clearing
    } catch (error) {
      console.error('Failed to clear cart:', error);
    }
  };

  const handleCheckout = () => {
    setShowPayment(true);
  };

  const handleConfirmPayment = async () => {
    try {
      const response = await fetch('https://localhost:7042/api/cart/checkout', {
        method: 'POST',
        credentials: 'include',
      });
      if (!response.ok) throw new Error('Failed to process payment');

      setPaymentSuccess(true);
      setShowPayment(false);

      // Clear cart after successful payment
      handleClearCart();
    } catch (error) {
      console.error('Payment failed:', error);
    }
  };

  return (
    <div>
      <h1 className="text-3xl font-bold text-center mb-8">Shopping Cart</h1>

      <div className="mb-4">
        <Link href="/">
          <button className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600 transition-colors duration-300">
            กลับสู่หน้ารายการสินค้า
          </button>
        </Link>
      </div>

      {/* แสดงข้อความแจ้งเตือนหากมีข้อผิดพลาด */}
      {errorMessage && (
        <div className="mb-4 text-center text-red-500 bg-red-100 border border-red-200 rounded p-2">
          {errorMessage}
        </div>
      )}

      {cartItems.length === 0 ? (
        <p className="text-center text-black">Your cart is empty</p>
      ) : (
        <div className="bg-white p-6 rounded-lg shadow-md">
          <div className="grid grid-cols-1 gap-4 mb-4">
            {cartItems.map((item) => (
              <div key={item.productId} className="border p-4 rounded shadow hover:shadow-lg transition-shadow duration-300">
                <h2 className="text-xl font-semibold text-black">{item.name}</h2>
                <p className="text-gray-600">Price: ${item.price}</p>
                <p className="text-gray-500">Quantity: {item.quantity}</p>
                <p className="text-gray-500">Stock: {item.stockQuantity}</p>
                <div className="flex items-center mt-2">
                  <button
                    className="bg-green-500 text-white px-4 py-2 rounded hover:bg-green-600 mr-2"
                    onClick={() => handleIncreaseQuantity(item.productId, item.name)}
                  >
                    +
                  </button>
                  <button
                    className="bg-red-500 text-white px-4 py-2 rounded hover:bg-red-600"
                    onClick={() => handleDecreaseQuantity(item.productId, item.name)}
                  >
                    -
                  </button>
                </div>
                <p className="text-gray-500 mt-2">Total: ${(item.price * item.quantity).toFixed(2)}</p>
                <button
                  className="mt-4 bg-red-500 text-white px-4 py-2 rounded hover:bg-red-600 transition-colors duration-300"
                  onClick={() => removeFromCart(item.productId)}
                >
                  Remove
                </button>
              </div>
            ))}
          </div>
          <div className="text-right mt-6">
            <p className="text-xl font-bold text-black">Total Amount: ${totalAmount.toFixed(2)}</p>
            <button
              className="mt-4 bg-blue-500 text-white px-6 py-3 rounded hover:bg-blue-600 transition-colors duration-300"
              onClick={handleCheckout}
            >
              Checkout
            </button>
          </div>
        </div>
      )}

      {showPayment && (
        <div className="fixed inset-0 flex items-center justify-center bg-gray-500 bg-opacity-75">
          <div className="bg-white p-8 rounded shadow">
            <h2 className="text-2xl font-bold mb-4 text-black">Confirm Payment</h2>
            <p className="mb-4 text-black">Total Amount to Pay: ${totalAmount.toFixed(2)}</p>
            <button
              className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600 mr-2"
              onClick={handleConfirmPayment}
            >
              Confirm Payment
            </button>
            <button
              className="bg-gray-300 text-black px-4 py-2 rounded hover:bg-gray-400"
              onClick={() => setShowPayment(false)}
            >
              Cancel
            </button>
          </div>
        </div>
      )}

      {paymentSuccess && (
        <div className="fixed inset-0 flex items-center justify-center bg-gray-500 bg-opacity-75">
          <div className="bg-white p-8 rounded shadow">
            <h2 className="text-2xl font-bold mb-4 text-black">Payment Successful!</h2>
            <p className="text-black">Your payment has been processed successfully.</p>
            <button
              className="mt-4 bg-green-500 text-white px-4 py-2 rounded hover:bg-green-600"
              onClick={() => setPaymentSuccess(false)}
            >
              Close
            </button>
          </div>
        </div>
      )}
    </div>
  );
}

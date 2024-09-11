import Link from 'next/link';

export default function Header() {
  return (
    <header className="bg-blue-600 text-white py-4 shadow-md">
      <div className="container mx-auto flex justify-between items-center px-4">
        <h1 className="text-2xl font-bold">
          <Link href="/">Shopping จริงๆ</Link>
        </h1>
        <nav>
          <Link href="/" className="text-white hover:text-gray-200 px-3 py-2 rounded-md text-sm font-medium">
            Home
          </Link>
          <Link href="/cart" className="text-white hover:text-gray-200 px-3 py-2 rounded-md text-sm font-medium">
            Cart
          </Link>
        </nav>
      </div>
    </header>
  );
}

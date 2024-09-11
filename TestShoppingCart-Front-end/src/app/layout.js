import './globals.css'; // Import CSS ทั่วไป
import Header from './components/Header';
import Footer from './components/Footer';

export const metadata = {
  title: 'My Shopping Site',
  description: 'An awesome shopping site built with Next.js 14',
};

export default function RootLayout({ children }) {
  return (
    <html lang="en">
      <body className="bg-gray-50 min-h-screen flex flex-col">
        <Header />
        <main className="flex-grow container mx-auto p-4">{children}</main>
        <Footer />
      </body>
    </html>
  );
}

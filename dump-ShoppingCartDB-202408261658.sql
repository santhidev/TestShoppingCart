PGDMP      :                |            ShoppingCartDB    16.4    16.4     �           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                      false            �           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                      false            �           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                      false            �           1262    16407    ShoppingCartDB    DATABASE     �   CREATE DATABASE "ShoppingCartDB" WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'English_United States.1252';
     DROP DATABASE "ShoppingCartDB";
                postgres    false                        2615    2200    public    SCHEMA        CREATE SCHEMA public;
    DROP SCHEMA public;
                pg_database_owner    false            �           0    0    SCHEMA public    COMMENT     6   COMMENT ON SCHEMA public IS 'standard public schema';
                   pg_database_owner    false    4            �            1259    16416    products    TABLE     �   CREATE TABLE public.products (
    product_id integer NOT NULL,
    name character varying(100) NOT NULL,
    price numeric(10,2) NOT NULL
);
    DROP TABLE public.products;
       public         heap    postgres    false    4            �            1259    16415    products_product_id_seq    SEQUENCE     �   CREATE SEQUENCE public.products_product_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 .   DROP SEQUENCE public.products_product_id_seq;
       public          postgres    false    4    216            �           0    0    products_product_id_seq    SEQUENCE OWNED BY     S   ALTER SEQUENCE public.products_product_id_seq OWNED BY public.products.product_id;
          public          postgres    false    215            �            1259    16423    stocks    TABLE     ~   CREATE TABLE public.stocks (
    stock_id integer NOT NULL,
    product_id integer NOT NULL,
    quantity integer NOT NULL
);
    DROP TABLE public.stocks;
       public         heap    postgres    false    4            �            1259    16422    stocks_stock_id_seq    SEQUENCE     �   CREATE SEQUENCE public.stocks_stock_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 *   DROP SEQUENCE public.stocks_stock_id_seq;
       public          postgres    false    4    218            �           0    0    stocks_stock_id_seq    SEQUENCE OWNED BY     K   ALTER SEQUENCE public.stocks_stock_id_seq OWNED BY public.stocks.stock_id;
          public          postgres    false    217            U           2604    16419    products product_id    DEFAULT     z   ALTER TABLE ONLY public.products ALTER COLUMN product_id SET DEFAULT nextval('public.products_product_id_seq'::regclass);
 B   ALTER TABLE public.products ALTER COLUMN product_id DROP DEFAULT;
       public          postgres    false    216    215    216            V           2604    16426    stocks stock_id    DEFAULT     r   ALTER TABLE ONLY public.stocks ALTER COLUMN stock_id SET DEFAULT nextval('public.stocks_stock_id_seq'::regclass);
 >   ALTER TABLE public.stocks ALTER COLUMN stock_id DROP DEFAULT;
       public          postgres    false    217    218    218            �          0    16416    products 
   TABLE DATA                 public          postgres    false    216          �          0    16423    stocks 
   TABLE DATA                 public          postgres    false    218   �       �           0    0    products_product_id_seq    SEQUENCE SET     E   SELECT pg_catalog.setval('public.products_product_id_seq', 5, true);
          public          postgres    false    215            �           0    0    stocks_stock_id_seq    SEQUENCE SET     A   SELECT pg_catalog.setval('public.stocks_stock_id_seq', 5, true);
          public          postgres    false    217            X           2606    16421    products products_pkey 
   CONSTRAINT     \   ALTER TABLE ONLY public.products
    ADD CONSTRAINT products_pkey PRIMARY KEY (product_id);
 @   ALTER TABLE ONLY public.products DROP CONSTRAINT products_pkey;
       public            postgres    false    216            Z           2606    16428    stocks stocks_pkey 
   CONSTRAINT     V   ALTER TABLE ONLY public.stocks
    ADD CONSTRAINT stocks_pkey PRIMARY KEY (stock_id);
 <   ALTER TABLE ONLY public.stocks DROP CONSTRAINT stocks_pkey;
       public            postgres    false    218            [           2606    16429    stocks stocks_product_id_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.stocks
    ADD CONSTRAINT stocks_product_id_fkey FOREIGN KEY (product_id) REFERENCES public.products(product_id);
 G   ALTER TABLE ONLY public.stocks DROP CONSTRAINT stocks_product_id_fkey;
       public          postgres    false    4696    216    218            �      x���v
Q���W((M��L�+(�O)M.)Vs�	uV�0�QPw,(�IU�Q0�35д��$B�P�Sb�虛���Ͽ(1/b���� ��%������/ 3/5�Ec�V.. �#R_      �   T   x���v
Q���W((M��L�+.�O�.Vs�	uV�0�Q "Mk.O�j�u��Uk��`J�Z# 2'J���Y�rq �>@     
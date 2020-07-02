# ProyectoLFYA - Generador Scanner
Proyecto de del curso Lenguajes Formales y Autómatas. Universidad Rafael Landívar 2020.

PROYECTO PRÁCTICO FASE I
GENERADOR DE SCANNER

Objetivo:
Dentro del análisis de los lenguajes Formales es importante conocer las fases del proceso de compilación, la finalidad del proyecto es que el estudiante comprenda la función de los analizadores léxico y sintáctico de un compilador a través de la generación de un programa que sea capaz de reconocer un lenguaje y finalmente evaluar si las palabras utilizadas están bien formadas de acuerdo con una gramática.
El proyecto consta de 3 fases, las cuales son dependientes, es decir, que para poder terminar la fase III, las anteriores deben estar completas, de lo contrario no se podrá entregar el funcionamiento completo.
 
Explicación de la gramática:

En la primera fase del proyecto es necesario la lectura de un archivo de texto llamado: GRAMATICA.txt el cual contiene la definición de la gramática.

Dicho archivo está compuesto de las siguientes partes:

1.	SETS: Contiene la definición abreviada de un conjunto de símbolos terminales, esta parte puede o no venir dentro del archivo, no es necesario que aparezca pero si aparece, debe poseer al menos un SET.
a.	Ejemplo
SETS
					LETRA   = 'A'..'Z'+'a'..'z'+'_'
					DIGITO  = '0'..'9'
					CHARSET = CHR(32)..CHR(254)
b.	Tomar en cuenta las siguientes características:
i.	La palabra SETS debe estar en mayúscula.
ii.	Los sets pueden estar concatenados a través del signo “+”, como muestra el set: LETRA.
iii.	Se puede utilizar la función CHR como lo muestra el set: CHARSET.
iv.	Puede haber muchos espacios en blanco entre el identificador, el símbolo “=” y la definición.
v.	Puede haber varios saltos de línea (Enters) entre un SET y otro.

2.	TOKENS: Los tokens representan los símbolos terminales y no terminales de la gramática, en esta fase no nos importa si un identificador ha sido declarado o no en los SETS, 
a.	Ejemplo
TOKENS
	TOKEN 1= DIGITO DIGITO *
	TOKEN 2='"' CHARSET '"'|''' CHARSET '''
	TOKEN  4  = '='
	TOKEN  5  = '<''>'
	TOKEN  6  = '<'
	TOKEN  7  = '>'
	TOKEN  8  = '>''='
	TOKEN  9  = '<''='
	TOKEN  10  = '+'
	TOKEN  11  = '-'
	TOKEN  12  = 'O''R'
	TOKEN  13  = '*'
	TOKEN  14  = 'A''N''D'
	TOKEN  15  = 'M''O''D'
	TOKEN  16  = 'D''I''V'
	TOKEN  17  = 'N''O''T'
	TOKEN  40  = '(''*'
	TOKEN  41  = '*'')'
	TOKEN  42  = ';'
	TOKEN  43  = '.'
	TOKEN  44  = '{'
	TOKEN  45  = '}'
	TOKEN  46  = '('
	TOKEN  47  = ')'
	TOKEN  48  = '['
	TOKEN  49  = ']'
	TOKEN  50  = '.''.'
	TOKEN  51  = ':'
	TOKEN  52  = ','
	TOKEN  53  = ':''='
	TOKEN 3= LETRA ( LETRA | DIGITO )*   { RESERVADAS() }
b.	LA PALABRA TOKENS debe existir y estar en mayúscula
c.	Esta sección debe existir
d.	Cada token debe poseer la palabra: TOKEN y un número, seguido del signo igual “=”.
e.	Después del signo igual debe venir una expresión regular, que puede ser uno o varios caracteres (Encerrados en apóstrofes).
f.	Los signos utilizados para las operaciones de las expresiones regulares son los únicos que no necesitan estar entre comillas, a menos que se quiera denotar su uso como signo terminal.
i.	Los signos de operaciones para las expresiones regulares son: + * ? ( ) | 

3.	ACTIONS: La palabra ACTIONS contiene definición de funciones, en este caso específico las palabras reservadas del lenguaje, es importante que la función: Reservadas() siempre debe existir y puede haber otras funciones.
a.	Ejemplo
ACTIONS 
RESERVADAS() 
{
	18 = 'PROGRAM'
	19 = 'INCLUDE'
	20 = 'CONST'
	21 = 'TYPE'
	22 = 'VAR'
	23 = 'RECORD'
	24 = 'ARRAY'
	25 = 'OF'
	26 = 'PROCEDURE'
	27 = 'FUNCTION'
	28 = 'IF'
	29 = 'THEN'
	30 = 'ELSE'
	31 = 'FOR'
	32 = 'TO'
	33 = 'WHILE'
	34 = 'DO'
	35 = 'EXIT'
	36 = 'END'
	37 = 'CASE'
	38 = 'BREAK'
	39 = 'DOWNTO'
}

b.	La palabra ACTIONS siempre debe venir acompañada de la función RESERVADAS ().
c.	Todas las funciones deben tener un identificador y unos paréntesis abierto y cerrado.
d.	Las funciones descritas en ACTIONS deben iniciar y finalizar con llaves {}.
e.	Los tokens dentro están conformados por: número, signo igual y luego el identificador entre apóstrofes

4.	ERROR: La definición de errores debe venir al menos uno, el ERROR debe tener asignado un número, y el identificador debe tener como sufjio la palabra ERROR en mayúscula:
a.	Ejemplo:
ERROR = 54
b.	Los identificadores solo deben poseer letras, y en la parte derecha del símbolo igual, solamente pueden haber números.



PROYECTO PRÁCTICO FASE II
GENERADOR DE SCANNER

Objetivo
En la Fase II del proyecto es pre-requisito haber entregado completamente la Fase I utilizando árboles de expresiones, ya que en esta fase, el entregable es la tabla de First, Last, Follow y también la tabla de transiciones del autómata Finito Determinista.
Estas tablas deben ser obtenidas de la sección “TOKENS” del archivo de entrad 
Explicación de la salida

En la segunda fase del proyecto es necesario la lectura de un archivo de texto llamado: GRAMATICA.txt el cual contiene la definición de la gramática.
 
Al leerlo el programa generará una cadena con la expresión regular proveniente de la sección tokens, por ejemplo utilizando el archivo anterior tenemos: 

DIGITO DIGITO * | '"' CHARSET '"' | ''' CHARSET ''' | '=' | '<' '>' | '<' | '>' | '>' '=' | '<' '=' | '+' | '-' | 'O' 'R' | '*' | 'A' 'N' 'D' | 'M' 'O' 'D' | 'D' 'I' 'V' | 'N' 'O' 'T' | '(' '*' | '*' ')' | ';' | '.' | '{' | '}' | '(' | ')' | '[' | ']' | '.' '.' | ':' | ',' | ':' '=' | LETRA ( LETRA | DIGITO ) *

Con esa expresión regular, se genera el árbol de expresiones y se obtiene la tabla de First, Last, Follow y la tabla de transiciones del Autómata Finito Determinista, para el ejemplo anterior tendríamos:



PROYECTO PRÁCTICO FASE III
GENERADOR DE SCANNER

Objetivo
En la Fase III del proyecto es pre-requisito haber entregado completamente la Fase I y la Fase II culminando con la tabla de transiciones, ya que en esta fase, el entregable el generador de scanner completo.
¿Qué es un generador de scanner?
Un scanner o analizador léxico es según Aho: “La primera fase de un compilador.  Su principal función consiste en leer los caracteres de entrada y elaborar como salida una secuencia  de componentes léxicos que utiliza el analizador sintáctico para hacer el análisis”[AHO].
De acuerdo a lo anterior nuestro programa es un generador de estos analizadores léxicos ya que a través de las expresiones regulares obtenidas en la sección “tokens” del archivo será capaz de generar programas que reconozcan un determinado lenguaje regular.
 
Explicación de la salida

En la tercera fase del proyecto es necesario la lectura de un archivo de texto llamado: GRAMATICA.txt el cual contiene la definición de la gramática.
 
Al determinar que los resultados de la fase I y fase II son correctos entonces se generará un programa en lenguaje C# el cual al ser compilado será capaz de reconocer un lenguaje regular (Definido por las expresiones regulares de la sección tokens). 

SET DE PRUEBAS:

Tenemos el siguiente archivo de prueba:

SETS
LETRA='A'..'Z'+'a'..'z'+'_'
DIGITO='0'..'9'
TOKENS
TOKEN 1=DIGITO DIGITO *
TOKEN 2='='
TOKEN 3=':''='
TOKEN 4=LETRA(LETRA|DIGITO)* {RESERVADAS()}
ACTIONS
RESERVADAS()
{
5='PROGRAM'
6='INCLUDE'
7='CONST'
8='TYPE'
}
ERROR=9

Esto generará un código en C# que al ser compilado y generar un programa .exe sea capaz de reconocer la siguiente línea de código:

Program x a:=b c=d const a

Dando como resultado lo siguiente:

Program = 5
x = 4
a = 4
:= = 3
b = 4
c = 4
= = 2
Const = 7
a = 4



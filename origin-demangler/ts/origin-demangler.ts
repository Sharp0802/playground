
class CaretRange {
    Node: Node;
    Begin: number;
    End: number;
    Term: string;

    private constructor(node: Node, begin: number, end: number, term: string) {
        this.Node = node;
        this.Begin = begin;
        this.End = end;
        this.Term = term;
    }

    static IsAcceptable(c: string): boolean {
        const zero = '0'.charCodeAt(0);
        const nine = '9'.charCodeAt(0);
    
        const smallA = 'a'.charCodeAt(0);
        const smallZ = 'z'.charCodeAt(0);
    
        const capitalA = 'A'.charCodeAt(0);
        const capitalZ = 'Z'.charCodeAt(0);
    
        const ch = c.charCodeAt(0);
        return (zero <= ch && ch <= nine)
            || (smallA <= ch && ch <= smallZ)
            || (capitalA <= ch && ch <= capitalZ);
    }

    static FromCursor(x: number, y: number) : CaretRange | null
    {
        const caret = (<any>document).caretPositionFromPoint(x, y);
    
        const node = caret.offsetNode;
        const offset = caret.offset;
    
        let begin = 0;
        for (let i = offset; i >= 0; --i) {
            if (!CaretRange.IsAcceptable(node.textContent[i])) {
                begin = i + 1;
                break;
            }
        }
    
        let str = <string>node.textContent;
        let end = str.length - 1;
        for (let i = offset; i < str.length; ++i) {
            if (!CaretRange.IsAcceptable(node.textContent[i])) {
                end = i;
                break;
            }
        }
    
        if (end < begin)
            return null;
    
        return new CaretRange(node, begin, end, str.substring(begin, end));
    }
}

function GetWord(e: MouseEvent) {
    const range = CaretRange.FromCursor(e.clientX, e.clientY);
    console.log(range.Term);
}

addEventListener("dblclick", GetWord);